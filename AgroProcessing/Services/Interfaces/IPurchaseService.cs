using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Purchase;

namespace AgroProcessing.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<PurchaseBatch> CreatePurchaseBatchAsync(CreatePurchaseBatchDto dto);
        Task<PurchaseBatch?> GetPurchaseBatchByIdAsync(Guid purchaseBatchId);
        Task<IEnumerable<PurchaseBatch>> GetPurchaseBatchesByFarmerAsync(Guid farmerId);
        Task<IEnumerable<PurchaseBatch>> GetPendingPurchaseBatchesAsync();
        Task<decimal> GetOutstandingAmountAsync(Guid purchaseBatchId);
        Task<bool> CanProcessBatchAsync(Guid purchaseBatchId);
    }
}
