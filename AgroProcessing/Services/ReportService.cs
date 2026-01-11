using AgroProcessing.Data;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetBatchProfitReportAsync(Guid purchaseBatchId)
        {
            var batch = await _context.PurchaseBatches.FindAsync(purchaseBatchId);
            if (batch == null)
                throw new InvalidOperationException("Purchase batch not found");

            var purchaseCost = batch.TotalAmount;

            var allocations = await _context.BatchOutputAllocations
                .Where(boa => boa.PurchaseBatchId == purchaseBatchId)
                .ToListAsync();

            var totalProcessingCost = allocations.Sum(a => a.AllocatedProcessingCost);
            var totalOutputWeight = allocations.Sum(a => a.OutputWeight);

            var transportCost = await _context.Transportations
                .Where(t => t.RelatedType == "Purchase" && t.RelatedId == purchaseBatchId)
                .SumAsync(t => t.Cost);

            var salesAllocations = await _context.SaleBatchAllocations
                .Where(sba => sba.PurchaseBatchId == purchaseBatchId)
                .ToListAsync();

            var saleItemIds = salesAllocations.Select(sa => sa.SaleItemId).ToList();
            var saleItems = await _context.SaleItems
                .Where(si => saleItemIds.Contains(si.SaleItemId))
                .ToListAsync();

            decimal totalRevenue = 0;
            foreach (var allocation in salesAllocations)
            {
                var saleItem = saleItems.FirstOrDefault(si => si.SaleItemId == allocation.SaleItemId);
                if (saleItem != null)
                {
                    totalRevenue += allocation.QuantityAllocated * saleItem.Rate;
                }
            }

            var totalCost = purchaseCost + totalProcessingCost + transportCost;
            var profit = totalRevenue - totalCost;
            var profitMargin = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0;

            return new
            {
                PurchaseBatchId = purchaseBatchId,
                PurchaseCost = purchaseCost,
                ProcessingCost = totalProcessingCost,
                TransportCost = transportCost,
                TotalCost = totalCost,
                TotalRevenue = totalRevenue,
                Profit = profit,
                ProfitMargin = profitMargin,
                OutputWeight = totalOutputWeight
            };
        }

        public async Task<object> GetYieldAnalysisReportAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            var processingRuns = await _context.ProcessingRuns
                .Where(pr => pr.ProductId == productId && pr.Status == "Completed")
                .Select(pr => pr.ProcessingRunId)
                .ToListAsync();

            var outputs = await _context.ProcessingOutputs
                .Where(po => processingRuns.Contains(po.ProcessingRunId))
                .ToListAsync();

            if (!outputs.Any())
            {
                return new
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    ExpectedYield = product.ExpectedYieldPercent,
                    Message = "No completed processing runs found"
                };
            }

            var avgYield = outputs.Average(o => o.YieldPercent);
            var minYield = outputs.Min(o => o.YieldPercent);
            var maxYield = outputs.Max(o => o.YieldPercent);
            var totalInput = outputs.Sum(o => o.TotalInputWeight);
            var totalOutput = outputs.Sum(o => o.TotalOutputWeight);
            var totalLoss = outputs.Sum(o => o.TotalLossWeight);

            return new
            {
                ProductId = productId,
                ProductName = product.Name,
                ExpectedYield = product.ExpectedYieldPercent,
                AverageActualYield = avgYield,
                MinYield = minYield,
                MaxYield = maxYield,
                TotalProcessingRuns = outputs.Count,
                TotalInputWeight = totalInput,
                TotalOutputWeight = totalOutput,
                TotalLossWeight = totalLoss,
                YieldVariance = avgYield - product.ExpectedYieldPercent
            };
        }

        public async Task<object> GetOutstandingPaymentsReportAsync()
        {
            var purchasePayments = await _context.PurchasePayments
                .Where(pp => pp.PaymentStatus == "Pending")
                .GroupBy(pp => pp.PurchaseBatchId)
                .Select(g => new
                {
                    PurchaseBatchId = g.Key,
                    TotalOutstanding = g.Sum(pp => pp.AmountPaid)
                })
                .ToListAsync();

            var salesPayments = await _context.SalesPayments
                .Where(sp => sp.PaymentStatus == "Pending")
                .GroupBy(sp => sp.SaleId)
                .Select(g => new
                {
                    SaleId = g.Key,
                    TotalOutstanding = g.Sum(sp => sp.AmountPaid)
                })
                .ToListAsync();

            var workerPayments = await _context.WorkerPayments
                .Where(wp => !wp.PaidStatus)
                .GroupBy(wp => wp.WorkerId)
                .Select(g => new
                {
                    WorkerId = g.Key,
                    TotalUnpaid = g.Sum(wp => wp.Amount)
                })
                .ToListAsync();

            return new
            {
                PurchasePaymentsOutstanding = purchasePayments,
                SalesPaymentsOutstanding = salesPayments,
                WorkerPaymentsOutstanding = workerPayments,
                TotalPurchaseOutstanding = purchasePayments.Sum(p => p.TotalOutstanding),
                TotalSalesOutstanding = salesPayments.Sum(s => s.TotalOutstanding),
                TotalWorkerOutstanding = workerPayments.Sum(w => w.TotalUnpaid)
            };
        }

        public async Task<object> GetInventoryValuationReportAsync()
        {
            var rawInventory = await _context.RawInventories
                .Where(ri => ri.Quantity > 0)
                .GroupBy(ri => ri.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(ri => ri.Quantity)
                })
                .ToListAsync();

            var finishedInventory = await _context.FinishedInventories
                .Where(fi => fi.Quantity > 0)
                .GroupBy(fi => fi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(fi => fi.Quantity)
                })
                .ToListAsync();

            return new
            {
                RawInventory = rawInventory,
                FinishedInventory = finishedInventory,
                TotalRawQuantity = rawInventory.Sum(r => r.TotalQuantity),
                TotalFinishedQuantity = finishedInventory.Sum(f => f.TotalQuantity)
            };
        }

        public async Task<object> GetWorkerPaymentSummaryAsync(Guid? workerId = null)
        {
            var query = _context.WorkerPayments.AsQueryable();

            if (workerId.HasValue)
            {
                query = query.Where(wp => wp.WorkerId == workerId.Value);
            }

            var summary = await query
                .GroupBy(wp => wp.WorkerId)
                .Select(g => new
                {
                    WorkerId = g.Key,
                    TotalEarned = g.Sum(wp => wp.Amount),
                    TotalPaid = g.Where(wp => wp.PaidStatus).Sum(wp => wp.Amount),
                    TotalUnpaid = g.Where(wp => !wp.PaidStatus).Sum(wp => wp.Amount),
                    PaymentCount = g.Count()
                })
                .ToListAsync();

            return summary;
        }

        public async Task<object> GetProcessingCostAnalysisAsync(Guid processingRunId)
        {
            var run = await _context.ProcessingRuns.FindAsync(processingRunId);
            if (run == null)
                throw new InvalidOperationException("Processing run not found");

            var stages = await _context.ProcessingStages
                .Where(ps => ps.ProcessingRunId == processingRunId)
                .ToListAsync();

            var stageIds = stages.Select(s => s.ProcessingStageId).ToList();

            var workerCosts = await _context.ProcessingStageWorkers
                .Where(psw => stageIds.Contains(psw.ProcessingStageId))
                .GroupBy(psw => psw.ProcessingStageId)
                .Select(g => new
                {
                    ProcessingStageId = g.Key,
                    TotalWorkerCost = g.Sum(psw => psw.CalculatedCost),
                    WorkerCount = g.Count()
                })
                .ToListAsync();

            var additionalCosts = await _context.ProcessingCosts
                .Where(pc => pc.ProcessingRunId == processingRunId)
                .ToListAsync();

            var totalWorkerCost = workerCosts.Sum(wc => wc.TotalWorkerCost);
            var totalAdditionalCost = additionalCosts.Sum(ac => ac.Amount);

            return new
            {
                ProcessingRunId = processingRunId,
                StageCount = stages.Count,
                TotalWorkerCost = totalWorkerCost,
                TotalAdditionalCost = totalAdditionalCost,
                TotalProcessingCost = totalWorkerCost + totalAdditionalCost,
                WorkerCostsByStage = workerCosts,
                AdditionalCosts = additionalCosts.Select(ac => new
                {
                    ac.CostType,
                    ac.Amount
                })
            };
        }
    }
}
