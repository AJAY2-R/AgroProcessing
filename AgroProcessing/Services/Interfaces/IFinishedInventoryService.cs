using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Services.Interfaces
{
    public interface IFinishedInventoryService
    {
        Task<FinishedInventory> AddFinishedStockAsync(Guid purchaseBatchId, Guid productId, Guid locationId, decimal quantity);
        Task<bool> ReduceFinishedStockAsync(Guid purchaseBatchId, decimal quantity);
        Task<decimal> GetAvailableFinishedQuantityAsync(Guid purchaseBatchId);
        Task<IEnumerable<FinishedInventory>> GetFinishedInventoryByLocationAsync(Guid locationId);
        Task<IEnumerable<FinishedInventory>> GetFinishedInventoryByProductAsync(Guid productId);
        Task<IEnumerable<FinishedInventory>> GetAvailableStockForSaleAsync(Guid productId);
        Task<bool> HasSufficientFinishedStockAsync(Guid productId, decimal requiredQuantity);
    }
}
