using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Processing;

namespace AgroProcessing.Services.Interfaces
{
    public interface IProcessingRunService
    {
        Task<ProcessingRun> CreateProcessingRunAsync(CreateProcessingRunDto dto);
        Task<ProcessingStage> AddProcessingStageAsync(Guid processingRunId, Guid workTypeId, DateTime startDate);
        Task<bool> CompleteProcessingStageAsync(Guid processingStageId, DateTime endDate);
        Task<ProcessingStageWorker> AssignWorkerToStageAsync(Guid processingStageId, Guid workerId, int workedDays);
        Task<ProcessingRun> CompleteProcessingRunAsync(CompleteProcessingRunDto dto);
        Task<ProcessingRun?> GetProcessingRunByIdAsync(Guid processingRunId);
        Task<IEnumerable<ProcessingRun>> GetActiveProcessingRunsAsync();
        Task<decimal> CalculateTotalProcessingCostAsync(Guid processingRunId);
    }
}
