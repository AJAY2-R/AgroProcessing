using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.DTOs.Purchase;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Purchase batch management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Purchases")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        /// <summary>
        /// Create new purchase batch
        /// </summary>
        /// <param name="dto">Purchase batch details including optional initial payment</param>
        /// <returns>Created purchase batch</returns>
        /// <response code="201">Purchase batch created successfully</response>
        /// <response code="400">Invalid input or business rule violation</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PurchaseBatch>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PurchaseBatch>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PurchaseBatch>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PurchaseBatch>>> CreatePurchaseBatch([FromBody] CreatePurchaseBatchDto dto)
        {
            try
            {
                var batch = await _purchaseService.CreatePurchaseBatchAsync(dto);
                return CreatedAtAction(nameof(GetPurchaseBatchById), new { id = batch.PurchaseBatchId },
                    ApiResponse<PurchaseBatch>.SuccessResponse(batch, "Purchase batch created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<PurchaseBatch>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<PurchaseBatch>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PurchaseBatch>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get purchase batch by ID
        /// </summary>
        /// <param name="id">Purchase batch unique identifier</param>
        /// <returns>Purchase batch with payment history</returns>
        /// <response code="200">Returns the purchase batch</response>
        /// <response code="404">Purchase batch not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PurchaseBatch>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PurchaseBatch>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PurchaseBatch>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PurchaseBatch>>> GetPurchaseBatchById(Guid id)
        {
            try
            {
                var batch = await _purchaseService.GetPurchaseBatchByIdAsync(id);
                if (batch == null)
                    return NotFound(ApiResponse<PurchaseBatch>.ErrorResponse("Purchase batch not found"));

                return Ok(ApiResponse<PurchaseBatch>.SuccessResponse(batch));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PurchaseBatch>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all purchase batches for a specific farmer
        /// </summary>
        /// <param name="farmerId">Farmer unique identifier</param>
        /// <returns>List of purchase batches</returns>
        /// <response code="200">Returns the list of purchase batches</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("farmer/{farmerId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchaseBatch>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchaseBatch>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseBatch>>>> GetPurchaseBatchesByFarmer(Guid farmerId)
        {
            try
            {
                var batches = await _purchaseService.GetPurchaseBatchesByFarmerAsync(farmerId);
                return Ok(ApiResponse<IEnumerable<PurchaseBatch>>.SuccessResponse(batches));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PurchaseBatch>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all pending purchase batches
        /// </summary>
        /// <returns>List of pending purchase batches</returns>
        /// <response code="200">Returns pending purchase batches</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("pending")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchaseBatch>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchaseBatch>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseBatch>>>> GetPendingPurchaseBatches()
        {
            try
            {
                var batches = await _purchaseService.GetPendingPurchaseBatchesAsync();
                return Ok(ApiResponse<IEnumerable<PurchaseBatch>>.SuccessResponse(batches));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PurchaseBatch>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get outstanding payment amount for a purchase batch
        /// </summary>
        /// <param name="id">Purchase batch unique identifier</param>
        /// <returns>Outstanding amount</returns>
        /// <response code="200">Returns the outstanding amount</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}/outstanding")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetOutstandingAmount(Guid id)
        {
            try
            {
                var outstanding = await _purchaseService.GetOutstandingAmountAsync(id);
                return Ok(ApiResponse<decimal>.SuccessResponse(outstanding));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Check if a purchase batch can be processed
        /// </summary>
        /// <param name="id">Purchase batch unique identifier</param>
        /// <returns>Boolean indicating if batch can be processed</returns>
        /// <response code="200">Returns processing eligibility</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}/can-process")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> CanProcessBatch(Guid id)
        {
            try
            {
                var canProcess = await _purchaseService.CanProcessBatchAsync(id);
                return Ok(ApiResponse<bool>.SuccessResponse(canProcess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }
}
