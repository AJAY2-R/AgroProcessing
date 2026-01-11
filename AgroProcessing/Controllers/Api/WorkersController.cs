using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Worker management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Master Data - Workers")]
    public class WorkersController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public WorkersController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// Get all active workers
        /// </summary>
        /// <returns>List of active workers</returns>
        /// <response code="200">Returns the list of active workers</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Worker>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Worker>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Worker>>>> GetAllWorkers()
        {
            try
            {
                var workers = await _masterDataService.GetActiveWorkersAsync();
                return Ok(ApiResponse<IEnumerable<Worker>>.SuccessResponse(workers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Worker>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get worker by ID
        /// </summary>
        /// <param name="id">Worker unique identifier</param>
        /// <returns>Worker details</returns>
        /// <response code="200">Returns the worker</response>
        /// <response code="404">Worker not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Worker>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Worker>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Worker>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Worker>>> GetWorkerById(Guid id)
        {
            try
            {
                var worker = await _masterDataService.GetWorkerByIdAsync(id);
                if (worker == null)
                    return NotFound(ApiResponse<Worker>.ErrorResponse("Worker not found"));

                return Ok(ApiResponse<Worker>.SuccessResponse(worker));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Worker>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Create new worker
        /// </summary>
        /// <param name="worker">Worker details</param>
        /// <returns>Created worker</returns>
        /// <response code="201">Worker created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Worker>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Worker>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Worker>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Worker>>> CreateWorker([FromBody] Worker worker)
        {
            try
            {
                var created = await _masterDataService.CreateWorkerAsync(worker);
                return CreatedAtAction(nameof(GetWorkerById), new { id = created.WorkerId },
                    ApiResponse<Worker>.SuccessResponse(created, "Worker created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Worker>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Worker>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Toggle worker active status
        /// </summary>
        /// <param name="id">Worker unique identifier</param>
        /// <returns>Result of toggle operation</returns>
        /// <response code="200">Status toggled successfully</response>
        /// <response code="404">Worker not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}/toggle-status")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> ToggleWorkerStatus(Guid id)
        {
            try
            {
                var result = await _masterDataService.ToggleWorkerStatusAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Worker not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(result, "Worker status toggled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }
}
