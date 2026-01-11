using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Services.Interfaces
{
    public interface IRawInventoryService
    {
        Task<RawInventory> AddStockAsync(Guid purchaseBatchId, Guid productId, Guid locationId, decimal quantity);
        Task<bool> ReduceStockAsync(Guid purchaseBatchId, decimal quantity);
        Task<decimal> GetAvailableQuantityAsync(Guid purchaseBatchId);
        Task<IEnumerable<RawInventory>> GetInventoryByLocationAsync(Guid locationId);
        Task<IEnumerable<RawInventory>> GetInventoryByProductAsync(Guid productId);
        Task<bool> HasSufficientStockAsync(Guid purchaseBatchId, decimal requiredQuantity);
    }
}
