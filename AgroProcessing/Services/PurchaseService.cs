using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Purchase;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;
        private readonly IRawInventoryService _rawInventoryService;

        public PurchaseService(AppDbContext context, IRawInventoryService rawInventoryService)
        {
            _context = context;
            _rawInventoryService = rawInventoryService;
        }

        public async Task<PurchaseBatch> CreatePurchaseBatchAsync(CreatePurchaseBatchDto dto)
        {
            if (dto.RawWeight <= 0)
                throw new ArgumentException("Raw weight must be greater than zero");

            if (dto.RatePerKg <= 0)
                throw new ArgumentException("Rate per kg must be greater than zero");

            var farmer = await _context.Farmers.FindAsync(dto.FarmerId);
            if (farmer == null)
                throw new InvalidOperationException("Farmer not found");

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            var totalAmount = dto.RawWeight * dto.RatePerKg;

            var purchaseBatch = new PurchaseBatch
            {
                PurchaseBatchId = Guid.NewGuid(),
                FarmerId = dto.FarmerId,
                ProductId = dto.ProductId,
                PurchaseDate = dto.PurchaseDate,
                RawWeight = dto.RawWeight,
                RatePerKg = dto.RatePerKg,
                TotalAmount = totalAmount,
                PaymentSettlementType = dto.PaymentSettlementType,
                Status = "Stored"
            };

            _context.PurchaseBatches.Add(purchaseBatch);
            await _context.SaveChangesAsync();

            if (dto.InitialPayment.HasValue && dto.InitialPayment.Value > 0)
            {
                if (dto.InitialPayment.Value > totalAmount)
                    throw new ArgumentException("Initial payment cannot exceed total amount");

                if (string.IsNullOrWhiteSpace(dto.PaymentMode))
                    throw new ArgumentException("Payment mode is required for initial payment");

                var payment = new PurchasePayment
                {
                    PurchasePaymentId = Guid.NewGuid(),
                    PurchaseBatchId = purchaseBatch.PurchaseBatchId,
                    AmountPaid = dto.InitialPayment.Value,
                    PaymentDate = dto.PurchaseDate,
                    DueDate = dto.PurchaseDate,
                    PaymentMode = dto.PaymentMode,
                    PaymentStatus = "Paid"
                };

                _context.PurchasePayments.Add(payment);
                await _context.SaveChangesAsync();
            }

            return purchaseBatch;
        }

        public async Task<PurchaseBatch?> GetPurchaseBatchByIdAsync(Guid purchaseBatchId)
        {
            return await _context.PurchaseBatches
                .Include(pb => pb.Payments)
                .FirstOrDefaultAsync(pb => pb.PurchaseBatchId == purchaseBatchId);
        }

        public async Task<IEnumerable<PurchaseBatch>> GetPurchaseBatchesByFarmerAsync(Guid farmerId)
        {
            return await _context.PurchaseBatches
                .Where(pb => pb.FarmerId == farmerId)
                .Include(pb => pb.Payments)
                .OrderByDescending(pb => pb.PurchaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseBatch>> GetPendingPurchaseBatchesAsync()
        {
            return await _context.PurchaseBatches
                .Where(pb => pb.Status == "Stored")
                .Include(pb => pb.Payments)
                .OrderBy(pb => pb.PurchaseDate)
                .ToListAsync();
        }

        public async Task<decimal> GetOutstandingAmountAsync(Guid purchaseBatchId)
        {
            var batch = await _context.PurchaseBatches
                .Include(pb => pb.Payments)
                .FirstOrDefaultAsync(pb => pb.PurchaseBatchId == purchaseBatchId);

            if (batch == null)
                return 0;

            var totalPaid = batch.Payments.Sum(p => p.AmountPaid);
            return batch.TotalAmount - totalPaid;
        }

        public async Task<bool> CanProcessBatchAsync(Guid purchaseBatchId)
        {
            var batch = await GetPurchaseBatchByIdAsync(purchaseBatchId);
            if (batch == null)
                return false;

            if (batch.Status != "Stored")
                return false;

            if (batch.PaymentSettlementType == "BeforeProcessing")
            {
                var outstanding = await GetOutstandingAmountAsync(purchaseBatchId);
                if (outstanding > 0)
                    return false;
            }

            var availableStock = await _rawInventoryService.GetAvailableQuantityAsync(purchaseBatchId);
            return availableStock > 0;
        }
    }
}
