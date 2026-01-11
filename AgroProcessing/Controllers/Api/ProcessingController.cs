using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.DTOs.Processing;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Processing run management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Processing")]
    public class ProcessingController : ControllerBase
    {
        private readonly IProcessingRunService _processingService;

        public ProcessingController(IProcessingRunService processingService)
        {
            _processingService = processingService;
        }

        /// <summary>
        /// Create a new processing run
        /// </summary>
        /// <param name="dto">Processing run details with input batches</param>
        /// <returns>Created processing run</returns>
        /// <response code="201">Processing run created successfully</response>
        /// <response code="400">Invalid input or insufficient raw material</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("runs")]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProcessingRun>>> CreateProcessingRun([FromBody] CreateProcessingRunDto dto)
        {
            try
            {
                var run = await _processingService.CreateProcessingRunAsync(dto);
                return CreatedAtAction(nameof(GetProcessingRunById), new { id = run.ProcessingRunId },
                    ApiResponse<ProcessingRun>.SuccessResponse(run, "Processing run created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get processing run by ID
        /// </summary>
        /// <param name="id">Processing run unique identifier</param>
        /// <returns>Processing run with stages and workers</returns>
        /// <response code="200">Returns the processing run</response>
        /// <response code="404">Processing run not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("runs/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProcessingRun>>> GetProcessingRunById(Guid id)
        {
            try
            {
                var run = await _processingService.GetProcessingRunByIdAsync(id);
                if (run == null)
                    return NotFound(ApiResponse<ProcessingRun>.ErrorResponse("Processing run not found"));

                return Ok(ApiResponse<ProcessingRun>.SuccessResponse(run));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all active (in-progress) processing runs
        /// </summary>
        /// <returns>List of active processing runs</returns>
        /// <response code="200">Returns active processing runs</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("runs/active")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProcessingRun>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProcessingRun>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProcessingRun>>>> GetActiveProcessingRuns()
        {
            try
            {
                var runs = await _processingService.GetActiveProcessingRunsAsync();
                return Ok(ApiResponse<IEnumerable<ProcessingRun>>.SuccessResponse(runs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ProcessingRun>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Add a processing stage to a run
        /// </summary>
        /// <param name="runId">Processing run unique identifier</param>
        /// <param name="dto">Stage details including work type and start date</param>
        /// <returns>Created processing stage</returns>
        /// <response code="200">Stage added successfully</response>
        /// <response code="400">Invalid input or run not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("runs/{runId}/stages")]
        [ProducesResponseType(typeof(ApiResponse<ProcessingStage>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingStage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingStage>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProcessingStage>>> AddProcessingStage(
            Guid runId, 
            [FromBody] AddStageDto dto)
        {
            try
            {
                var stage = await _processingService.AddProcessingStageAsync(runId, dto.WorkTypeId, dto.StartDate);
                return Ok(ApiResponse<ProcessingStage>.SuccessResponse(stage, "Stage added successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ProcessingStage>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProcessingStage>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Mark a processing stage as completed
        /// </summary>
        /// <param name="stageId">Processing stage unique identifier</param>
        /// <param name="endDate">Stage completion date</param>
        /// <returns>Result of operation</returns>
        /// <response code="200">Stage completed successfully</response>
        /// <response code="400">Invalid input or stage not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("stages/{stageId}/complete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> CompleteProcessingStage(Guid stageId, [FromBody] DateTime endDate)
        {
            try
            {
                var result = await _processingService.CompleteProcessingStageAsync(stageId, endDate);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Stage completed successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Assign a worker to a processing stage
        /// </summary>
        /// <param name="stageId">Processing stage unique identifier</param>
        /// <param name="dto">Worker assignment details including days worked</param>
        /// <returns>Created worker assignment</returns>
        /// <response code="200">Worker assigned successfully</response>
        /// <response code="400">Invalid input or duplicate assignment</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("stages/{stageId}/workers")]
        [ProducesResponseType(typeof(ApiResponse<ProcessingStageWorker>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingStageWorker>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingStageWorker>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProcessingStageWorker>>> AssignWorkerToStage(
            Guid stageId,
            [FromBody] AssignWorkerDto dto)
        {
            try
            {
                var assignment = await _processingService.AssignWorkerToStageAsync(stageId, dto.WorkerId, dto.WorkedDays);
                return Ok(ApiResponse<ProcessingStageWorker>.SuccessResponse(assignment, "Worker assigned successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<ProcessingStageWorker>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ProcessingStageWorker>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProcessingStageWorker>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Complete a processing run with output and costs
        /// </summary>
        /// <param name="dto">Completion details including output weight and additional costs</param>
        /// <returns>Completed processing run</returns>
        /// <response code="200">Processing run completed successfully</response>
        /// <response code="400">Invalid input or yield exceeds limits</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("runs/complete")]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<ProcessingRun>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ProcessingRun>>> CompleteProcessingRun([FromBody] CompleteProcessingRunDto dto)
        {
            try
            {
                var run = await _processingService.CompleteProcessingRunAsync(dto);
                return Ok(ApiResponse<ProcessingRun>.SuccessResponse(run, "Processing run completed successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProcessingRun>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Calculate total processing cost for a run
        /// </summary>
        /// <param name="runId">Processing run unique identifier</param>
        /// <returns>Total processing cost including worker payments and additional costs</returns>
        /// <response code="200">Returns the total processing cost</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("runs/{runId}/cost")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalProcessingCost(Guid runId)
        {
            try
            {
                var cost = await _processingService.CalculateTotalProcessingCostAsync(runId);
                return Ok(ApiResponse<decimal>.SuccessResponse(cost));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }
    }

    /// <summary>
    /// Data transfer object for adding a processing stage
    /// </summary>
    public class AddStageDto
    {
        /// <summary>
        /// Work type unique identifier (e.g., Cleaning, Drying, Sorting)
        /// </summary>
        public Guid WorkTypeId { get; set; }
        
        /// <summary>
        /// Stage start date
        /// </summary>
        public DateTime StartDate { get; set; }
    }

    /// <summary>
    /// Data transfer object for assigning a worker to a stage
    /// </summary>
    public class AssignWorkerDto
    {
        /// <summary>
        /// Worker unique identifier
        /// </summary>
        public Guid WorkerId { get; set; }
        
        /// <summary>
        /// Number of days worked
        /// </summary>
        public int WorkedDays { get; set; }
    }
}
