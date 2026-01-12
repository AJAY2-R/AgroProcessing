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
                .AsNoTracking()
                .Where(ri => ri.PurchaseBatchId == purchaseBatchId)
                .SumAsync(ri => ri.Quantity);
        }

        public async Task<IEnumerable<RawInventory>> GetInventoryByLocationAsync(Guid locationId)
        {
            return await _context.RawInventories
                .AsNoTracking()
                .Where(ri => ri.LocationId == locationId && ri.Quantity > 0)
                .OrderByDescending(ri => ri.Quantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<RawInventory>> GetInventoryByProductAsync(Guid productId)
        {
            return await _context.RawInventories
                .AsNoTracking()
                .Where(ri => ri.ProductId == productId && ri.Quantity > 0)
                .OrderByDescending(ri => ri.Quantity)
                .ToListAsync();
        }

        public async Task<bool> HasSufficientStockAsync(Guid purchaseBatchId, decimal requiredQuantity)
        {
            var available = await GetAvailableQuantityAsync(purchaseBatchId);
            return available >= requiredQuantity;
        }

        public async Task<IEnumerable<RawInventory>> GetAllInventoryAsync()
        {
            return await _context.RawInventories
                .AsNoTracking()
                .Where(ri => ri.Quantity > 0)
                .OrderByDescending(ri => ri.Quantity)
                .ToListAsync();
        }

        public async Task<object> GetInventorySummaryAsync()
        {
            var allInventory = await _context.RawInventories
                .AsNoTracking()
                .Where(ri => ri.Quantity > 0)
                .ToListAsync();

            var totalQuantity = allInventory.Sum(ri => ri.Quantity);
            var totalBatches = allInventory.Select(ri => ri.PurchaseBatchId).Distinct().Count();
            var totalLocations = allInventory.Select(ri => ri.LocationId).Distinct().Count();
            var totalProducts = allInventory.Select(ri => ri.ProductId).Distinct().Count();

            var productSummary = allInventory
                .GroupBy(ri => ri.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(ri => ri.Quantity),
                    LocationCount = g.Select(ri => ri.LocationId).Distinct().Count()
                })
                .ToList();

            var locationSummary = allInventory
                .GroupBy(ri => ri.LocationId)
                .Select(g => new
                {
                    LocationId = g.Key,
                    TotalQuantity = g.Sum(ri => ri.Quantity),
                    ProductCount = g.Select(ri => ri.ProductId).Distinct().Count()
                })
                .ToList();

            return new
            {
                TotalQuantity = totalQuantity,
                TotalBatches = totalBatches,
                TotalLocations = totalLocations,
                TotalProducts = totalProducts,
                AverageQuantityPerBatch = totalBatches > 0 ? totalQuantity / totalBatches : 0,
                ProductSummary = productSummary,
                LocationSummary = locationSummary
            };
        }

        public async Task<IEnumerable<RawInventory>> GetInventoryByBatchAsync(Guid purchaseBatchId)
        {
            return await _context.RawInventories
                .AsNoTracking()
                .Where(ri => ri.PurchaseBatchId == purchaseBatchId)
                .OrderByDescending(ri => ri.Quantity)
                .ToListAsync();
        }
    }
}
