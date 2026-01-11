using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class FinishedInventoryService : IFinishedInventoryService
    {
        private readonly AppDbContext _context;

        public FinishedInventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FinishedInventory> AddFinishedStockAsync(Guid purchaseBatchId, Guid productId, Guid locationId, decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var existingInventory = await _context.FinishedInventories
                .FirstOrDefaultAsync(fi => fi.PurchaseBatchId == purchaseBatchId && fi.LocationId == locationId);

            if (existingInventory != null)
            {
                existingInventory.Quantity += quantity;
                await _context.SaveChangesAsync();
                return existingInventory;
            }

            var inventory = new FinishedInventory
            {
                FinishedInventoryId = Guid.NewGuid(),
                PurchaseBatchId = purchaseBatchId,
                ProductId = productId,
                LocationId = locationId,
                Quantity = quantity
            };

            _context.FinishedInventories.Add(inventory);
            await _context.SaveChangesAsync();

            return inventory;
        }

        public async Task<bool> ReduceFinishedStockAsync(Guid purchaseBatchId, decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var inventories = await _context.FinishedInventories
                .Where(fi => fi.PurchaseBatchId == purchaseBatchId)
                .ToListAsync();

            var totalAvailable = inventories.Sum(fi => fi.Quantity);

            if (totalAvailable < quantity)
                throw new InvalidOperationException($"Insufficient finished stock. Available: {totalAvailable}, Required: {quantity}");

            var remaining = quantity;
            foreach (var inventory in inventories.OrderBy(fi => fi.Quantity))
            {
                if (remaining <= 0)
                    break;

                if (inventory.Quantity >= remaining)
                {
                    inventory.Quantity -= remaining;
                    remaining = 0;
                }
                else
                {
                    remaining -= inventory.Quantity;
                    inventory.Quantity = 0;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetAvailableFinishedQuantityAsync(Guid purchaseBatchId)
        {
            return await _context.FinishedInventories
                .Where(fi => fi.PurchaseBatchId == purchaseBatchId)
                .SumAsync(fi => fi.Quantity);
        }

        public async Task<IEnumerable<FinishedInventory>> GetFinishedInventoryByLocationAsync(Guid locationId)
        {
            return await _context.FinishedInventories
                .Where(fi => fi.LocationId == locationId && fi.Quantity > 0)
                .OrderByDescending(fi => fi.Quantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<FinishedInventory>> GetFinishedInventoryByProductAsync(Guid productId)
        {
            return await _context.FinishedInventories
                .Where(fi => fi.ProductId == productId && fi.Quantity > 0)
                .OrderByDescending(fi => fi.Quantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<FinishedInventory>> GetAvailableStockForSaleAsync(Guid productId)
        {
            return await _context.FinishedInventories
                .Where(fi => fi.ProductId == productId && fi.Quantity > 0)
                .OrderBy(fi => fi.PurchaseBatchId)
                .ToListAsync();
        }

        public async Task<bool> HasSufficientFinishedStockAsync(Guid productId, decimal requiredQuantity)
        {
            var totalAvailable = await _context.FinishedInventories
                .Where(fi => fi.ProductId == productId)
                .SumAsync(fi => fi.Quantity);

            return totalAvailable >= requiredQuantity;
        }
    }
}
