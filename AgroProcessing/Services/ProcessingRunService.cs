using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Processing;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class ProcessingRunService : IProcessingRunService
    {
        private readonly AppDbContext _context;
        private readonly IRawInventoryService _rawInventoryService;
        private readonly IFinishedInventoryService _finishedInventoryService;

        public ProcessingRunService(
            AppDbContext context,
            IRawInventoryService rawInventoryService,
            IFinishedInventoryService finishedInventoryService)
        {
            _context = context;
            _rawInventoryService = rawInventoryService;
            _finishedInventoryService = finishedInventoryService;
        }

        public async Task<ProcessingRun> CreateProcessingRunAsync(CreateProcessingRunDto dto)
        {
            if (dto.Inputs == null || !dto.Inputs.Any())
                throw new ArgumentException("At least one input is required");

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            foreach (var input in dto.Inputs)
            {
                if (input.InputWeight <= 0)
                    throw new ArgumentException("Input weight must be greater than zero");

                var hasStock = await _rawInventoryService.HasSufficientStockAsync(
                    input.PurchaseBatchId, input.InputWeight);

                if (!hasStock)
                    throw new InvalidOperationException(
                        $"Insufficient stock for purchase batch {input.PurchaseBatchId}");
            }

            var processingRun = new ProcessingRun
            {
                ProcessingRunId = Guid.NewGuid(),
                ProductId = dto.ProductId,
                StartDate = dto.StartDate,
                Status = "Started"
            };

            _context.ProcessingRuns.Add(processingRun);
            await _context.SaveChangesAsync();

            foreach (var input in dto.Inputs)
            {
                var runInput = new ProcessingRunInput
                {
                    ProcessingRunInputId = Guid.NewGuid(),
                    ProcessingRunId = processingRun.ProcessingRunId,
                    PurchaseBatchId = input.PurchaseBatchId,
                    InputWeight = input.InputWeight
                };

                _context.ProcessingRunInputs.Add(runInput);

                await _rawInventoryService.ReduceStockAsync(input.PurchaseBatchId, input.InputWeight);
            }

            await _context.SaveChangesAsync();
            return processingRun;
        }

        public async Task<ProcessingStage> AddProcessingStageAsync(Guid processingRunId, Guid workTypeId, DateTime startDate)
        {
            var run = await _context.ProcessingRuns.FindAsync(processingRunId);
            if (run == null)
                throw new InvalidOperationException("Processing run not found");

            if (run.Status != "Started")
                throw new InvalidOperationException("Processing run is not active");

            var workType = await _context.WorkTypes.FindAsync(workTypeId);
            if (workType == null)
                throw new InvalidOperationException("Work type not found");

            var stage = new ProcessingStage
            {
                ProcessingStageId = Guid.NewGuid(),
                ProcessingRunId = processingRunId,
                WorkTypeId = workTypeId,
                StartDate = startDate
            };

            _context.ProcessingStages.Add(stage);
            await _context.SaveChangesAsync();

            return stage;
        }

        public async Task<bool> CompleteProcessingStageAsync(Guid processingStageId, DateTime endDate)
        {
            var stage = await _context.ProcessingStages.FindAsync(processingStageId);
            if (stage == null)
                return false;

            if (stage.EndDate.HasValue)
                throw new InvalidOperationException("Stage is already completed");

            stage.EndDate = endDate;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProcessingStageWorker> AssignWorkerToStageAsync(Guid processingStageId, Guid workerId, int workedDays)
        {
            if (workedDays <= 0)
                throw new ArgumentException("Worked days must be greater than zero");

            var stage = await _context.ProcessingStages.FindAsync(processingStageId);
            if (stage == null)
                throw new InvalidOperationException("Processing stage not found");

            var worker = await _context.Workers.FindAsync(workerId);
            if (worker == null)
                throw new InvalidOperationException("Worker not found");

            var calculatedCost = worker.DefaultRate * workedDays;

            var stageWorker = new ProcessingStageWorker
            {
                ProcessingStageWorkerId = Guid.NewGuid(),
                ProcessingStageId = processingStageId,
                WorkerId = workerId,
                WorkedDays = workedDays,
                CalculatedCost = calculatedCost
            };

            _context.ProcessingStageWorkers.Add(stageWorker);
            await _context.SaveChangesAsync();

            return stageWorker;
        }

        public async Task<ProcessingRun> CompleteProcessingRunAsync(CompleteProcessingRunDto dto)
        {
            var run = await _context.ProcessingRuns
                .Include(pr => pr)
                .FirstOrDefaultAsync(pr => pr.ProcessingRunId == dto.ProcessingRunId);

            if (run == null)
                throw new InvalidOperationException("Processing run not found");

            if (run.Status != "Started")
                throw new InvalidOperationException("Processing run is not active");

            if (dto.TotalOutputWeight <= 0)
                throw new ArgumentException("Total output weight must be greater than zero");

            var inputs = await _context.ProcessingRunInputs
                .Where(pri => pri.ProcessingRunId == dto.ProcessingRunId)
                .ToListAsync();

            var totalInputWeight = inputs.Sum(i => i.InputWeight);
            var totalLossWeight = totalInputWeight - dto.TotalOutputWeight;
            var yieldPercent = (dto.TotalOutputWeight / totalInputWeight) * 100;

            var output = new ProcessingOutput
            {
                ProcessingOutputId = Guid.NewGuid(),
                ProcessingRunId = dto.ProcessingRunId,
                TotalInputWeight = totalInputWeight,
                TotalOutputWeight = dto.TotalOutputWeight,
                TotalLossWeight = totalLossWeight,
                YieldPercent = yieldPercent
            };

            _context.ProcessingOutputs.Add(output);

            foreach (var cost in dto.AdditionalCosts)
            {
                var processingCost = new ProcessingCost
                {
                    ProcessingCostId = Guid.NewGuid(),
                    ProcessingRunId = dto.ProcessingRunId,
                    CostType = cost.CostType,
                    Amount = cost.Amount
                };

                _context.ProcessingCosts.Add(processingCost);
            }

            var totalCost = await CalculateTotalProcessingCostAsync(dto.ProcessingRunId);
            totalCost += dto.AdditionalCosts.Sum(c => c.Amount);

            foreach (var input in inputs)
            {
                var proportionOfInput = input.InputWeight / totalInputWeight;
                var allocatedOutput = dto.TotalOutputWeight * proportionOfInput;
                var allocatedCost = totalCost * proportionOfInput;

                var allocation = new BatchOutputAllocation
                {
                    BatchOutputAllocationId = Guid.NewGuid(),
                    ProcessingRunId = dto.ProcessingRunId,
                    PurchaseBatchId = input.PurchaseBatchId,
                    InputWeight = input.InputWeight,
                    OutputWeight = allocatedOutput,
                    AllocatedProcessingCost = allocatedCost
                };

                _context.BatchOutputAllocations.Add(allocation);

                var batch = await _context.PurchaseBatches.FindAsync(input.PurchaseBatchId);
                if (batch != null)
                {
                    var location = await _context.Locations
                        .FirstOrDefaultAsync(l => l.LocationType == "Finished");

                    if (location != null)
                    {
                        await _finishedInventoryService.AddFinishedStockAsync(
                            input.PurchaseBatchId,
                            run.ProductId,
                            location.LocationId,
                            allocatedOutput);
                    }
                }
            }

            run.EndDate = dto.EndDate;
            run.Status = "Completed";

            await _context.SaveChangesAsync();
            return run;
        }

        public async Task<ProcessingRun?> GetProcessingRunByIdAsync(Guid processingRunId)
        {
            return await _context.ProcessingRuns
                .AsNoTracking()
                .FirstOrDefaultAsync(pr => pr.ProcessingRunId == processingRunId);
        }

        public async Task<IEnumerable<ProcessingRun>> GetActiveProcessingRunsAsync()
        {
            return await _context.ProcessingRuns
                .AsNoTracking()
                .Where(pr => pr.Status == "Started")
                .OrderBy(pr => pr.StartDate)
                .ToListAsync();
        }

        public async Task<decimal> CalculateTotalProcessingCostAsync(Guid processingRunId)
        {
            var stages = await _context.ProcessingStages
                .AsNoTracking()
                .Where(ps => ps.ProcessingRunId == processingRunId)
                .Select(ps => ps.ProcessingStageId)
                .ToListAsync();

            var workerCosts = await _context.ProcessingStageWorkers
                .AsNoTracking()
                .Where(psw => stages.Contains(psw.ProcessingStageId))
                .SumAsync(psw => psw.CalculatedCost);

            var additionalCosts = await _context.ProcessingCosts
                .AsNoTracking()
                .Where(pc => pc.ProcessingRunId == processingRunId)
                .SumAsync(pc => pc.Amount);

            return workerCosts + additionalCosts;
        }
    }
}
