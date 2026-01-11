using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Services.Interfaces
{
    public interface IWorkerPaymentService
    {
        Task<WorkerPayment> CreateWorkerPaymentAsync(Guid workerId, Guid processingStageId, decimal amount);
        Task<IEnumerable<WorkerPayment>> GetUnpaidWorkerPaymentsAsync();
        Task<IEnumerable<WorkerPayment>> GetWorkerPaymentsByWorkerAsync(Guid workerId);
        Task<bool> MarkWorkerPaymentAsPaidAsync(Guid workerPaymentId, DateTime paymentDate);
        Task<decimal> GetTotalUnpaidForWorkerAsync(Guid workerId);
        Task<IEnumerable<WorkerPayment>> GetPaymentsByProcessingStageAsync(Guid processingStageId);
    }
}
