using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Sales;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class SalesService : ISalesService
    {
        private readonly AppDbContext _context;
        private readonly IFinishedInventoryService _finishedInventoryService;

        public SalesService(AppDbContext context, IFinishedInventoryService finishedInventoryService)
        {
            _context = context;
            _finishedInventoryService = finishedInventoryService;
        }

        public async Task<Sale> CreateSaleAsync(CreateSaleDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("At least one sale item is required");

            var buyer = await _context.Buyers.FindAsync(dto.BuyerId);
            if (buyer == null)
                throw new InvalidOperationException("Buyer not found");

            var totalAmount = dto.Items.Sum(i => i.Quantity * i.Rate);

            var canProceed = await ValidateBuyerCreditLimitAsync(dto.BuyerId, totalAmount);
            if (!canProceed)
                throw new InvalidOperationException("Buyer has exceeded credit limit");

            foreach (var item in dto.Items)
            {
                var hasStock = await _finishedInventoryService.HasSufficientFinishedStockAsync(
                    item.ProductId, item.Quantity);

                if (!hasStock)
                    throw new InvalidOperationException($"Insufficient stock for product {item.ProductId}");
            }

            var sale = new Sale
            {
                SaleId = Guid.NewGuid(),
                BuyerId = dto.BuyerId,
                SaleDate = dto.SaleDate,
                TotalAmount = totalAmount,
                Status = "Open"
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            foreach (var itemDto in dto.Items)
            {
                var saleItem = new SaleItem
                {
                    SaleItemId = Guid.NewGuid(),
                    SaleId = sale.SaleId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Rate = itemDto.Rate
                };

                _context.SaleItems.Add(saleItem);
                await _context.SaveChangesAsync();

                var availableStock = await _finishedInventoryService.GetAvailableStockForSaleAsync(itemDto.ProductId);
                var remainingQty = itemDto.Quantity;

                foreach (var stock in availableStock)
                {
                    if (remainingQty <= 0)
                        break;

                    var allocateQty = Math.Min(stock.Quantity, remainingQty);

                    var allocation = new SaleBatchAllocation
                    {
                        SaleBatchAllocationId = Guid.NewGuid(),
                        SaleItemId = saleItem.SaleItemId,
                        PurchaseBatchId = stock.PurchaseBatchId,
                        QuantityAllocated = allocateQty
                    };

                    _context.SaleBatchAllocations.Add(allocation);

                    await _finishedInventoryService.ReduceFinishedStockAsync(stock.PurchaseBatchId, allocateQty);

                    remainingQty -= allocateQty;
                }
            }

            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale?> GetSaleByIdAsync(Guid saleId)
        {
            return await _context.Sales
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SaleId == saleId);
        }

        public async Task<IEnumerable<Sale>> GetSalesByBuyerAsync(Guid buyerId)
        {
            return await _context.Sales
                .AsNoTracking()
                .Where(s => s.BuyerId == buyerId)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetOpenSalesAsync()
        {
            return await _context.Sales
                .AsNoTracking()
                .Where(s => s.Status == "Open")
                .OrderBy(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<bool> CloseSaleAsync(Guid saleId)
        {
            var sale = await _context.Sales.FindAsync(saleId);
            if (sale == null)
                return false;

            sale.Status = "Closed";
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateBuyerCreditLimitAsync(Guid buyerId, decimal saleAmount)
        {
            var buyer = await _context.Buyers.FindAsync(buyerId);
            if (buyer == null)
                return false;

            var outstanding = await GetBuyerOutstandingAsync(buyerId);
            return (outstanding + saleAmount) <= buyer.CreditLimit;
        }

        public async Task<decimal> GetBuyerOutstandingAsync(Guid buyerId)
        {
            var sales = await _context.Sales
                .AsNoTracking()
                .Where(s => s.BuyerId == buyerId && s.Status == "Open")
                .Select(s => new { s.SaleId, s.TotalAmount })
                .ToListAsync();

            decimal totalOutstanding = 0;

            foreach (var sale in sales)
            {
                var payments = await _context.SalesPayments
                    .AsNoTracking()
                    .Where(sp => sp.SaleId == sale.SaleId)
                    .SumAsync(sp => sp.AmountPaid);

                totalOutstanding += (sale.TotalAmount - payments);
            }

            return totalOutstanding;
        }
    }
}
