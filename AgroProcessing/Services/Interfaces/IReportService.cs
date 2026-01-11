namespace AgroProcessing.Services.Interfaces
{
    public interface IReportService
    {
        Task<object> GetBatchProfitReportAsync(Guid purchaseBatchId);
        Task<object> GetYieldAnalysisReportAsync(Guid productId);
        Task<object> GetOutstandingPaymentsReportAsync();
        Task<object> GetInventoryValuationReportAsync();
        Task<object> GetWorkerPaymentSummaryAsync(Guid? workerId = null);
        Task<object> GetProcessingCostAnalysisAsync(Guid processingRunId);
    }
}
