using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class RawInventoryService : IRawInventoryService
    {
        private readonly AppDbContext _context;

        public RawInventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RawInventory> AddStockAsync(Guid purchaseBatchId, Guid productId, Guid locationId, decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var existingInventory = await _context.RawInventories
                .FirstOrDefaultAsync(ri => ri.PurchaseBatchId == purchaseBatchId && ri.LocationId == locationId);

            if (existingInventory != null)
            {
                existingInventory.Quantity += quantity;
                await _context.SaveChangesAsync();
                return existingInventory;
            }

            var inventory = new RawInventory
            {
                RawInventoryId = Guid.NewGuid(),
                PurchaseBatchId = purchaseBatchId,
                ProductId = productId,
                LocationId = locationId,
                Quantity = quantity
            };

            _context.RawInventories.Add(inventory);
            await _context.SaveChangesAsync();

            return inventory;
        }

        public async Task<bool> ReduceStockAsync(Guid purchaseBatchId, decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var inventories = await _context.RawInventories
                .Where(ri => ri.PurchaseBatchId == purchaseBatchId)
                .ToListAsync();

            var totalAvailable = inventories.Sum(ri => ri.Quantity);

            if (totalAvailable < quantity)
                throw new InvalidOperationException($"Insufficient stock. Available: {totalAvailable}, Required: {quantity}");

            var remaining = quantity;
            foreach (var inventory in inventories.OrderBy(ri => ri.Quantity))
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

        public async Task<decimal> GetAvailableQuantityAsync(Guid purchaseBatchId)
        {
            return await _context.RawInventories
                .Where(ri => ri.PurchaseBatchId == purchaseBatchId)
                .SumAsync(ri => ri.Quantity);
        }

        public async Task<IEnumerable<RawInventory>> GetInventoryByLocationAsync(Guid locationId)
        {
            return await _context.RawInventories
                .Where(ri => ri.LocationId == locationId && ri.Quantity > 0)
                .OrderByDescending(ri => ri.Quantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<RawInventory>> GetInventoryByProductAsync(Guid productId)
        {
            return await _context.RawInventories
                .Where(ri => ri.ProductId == productId && ri.Quantity > 0)
                .OrderByDescending(ri => ri.Quantity)
                .ToListAsync();
        }

        public async Task<bool> HasSufficientStockAsync(Guid purchaseBatchId, decimal requiredQuantity)
        {
            var available = await GetAvailableQuantityAsync(purchaseBatchId);
            return available >= requiredQuantity;
        }
    }
}
