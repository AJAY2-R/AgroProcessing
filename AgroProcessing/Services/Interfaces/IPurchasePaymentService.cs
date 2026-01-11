using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Purchase;

namespace AgroProcessing.Services.Interfaces
{
    public interface IPurchasePaymentService
    {
        Task<PurchasePayment> RecordPaymentAsync(RecordPaymentDto dto);
        Task<IEnumerable<PurchasePayment>> GetPaymentsByBatchAsync(Guid purchaseBatchId);
        Task<decimal> GetTotalPaidAsync(Guid purchaseBatchId);
        Task<IEnumerable<PurchasePayment>> GetOverduePaymentsAsync();
        Task<bool> MarkPaymentAsPaidAsync(Guid paymentId, DateTime paymentDate);
    }
}
