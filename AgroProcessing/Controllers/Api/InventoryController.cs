using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Inventory management endpoints for raw and finished goods
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IRawInventoryService _rawInventoryService;
        private readonly IFinishedInventoryService _finishedInventoryService;

        public InventoryController(
            IRawInventoryService rawInventoryService,
            IFinishedInventoryService finishedInventoryService)
        {
            _rawInventoryService = rawInventoryService;
            _finishedInventoryService = finishedInventoryService;
        }

        /// <summary>
        /// Get raw inventory by location
        /// </summary>
        /// <param name="locationId">Location unique identifier</param>
        /// <returns>List of raw inventory items at the location</returns>
        /// <response code="200">Returns raw inventory items</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("raw/location/{locationId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RawInventory>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RawInventory>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RawInventory>>>> GetRawInventoryByLocation(Guid locationId)
        {
            try
            {
                var inventory = await _rawInventoryService.GetInventoryByLocationAsync(locationId);
                return Ok(ApiResponse<IEnumerable<RawInventory>>.SuccessResponse(inventory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<RawInventory>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get raw inventory by product
        /// </summary>
        /// <param name="productId">Product unique identifier</param>
        /// <returns>List of raw inventory items for the product</returns>
        /// <response code="200">Returns raw inventory items</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("raw/product/{productId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RawInventory>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RawInventory>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RawInventory>>>> GetRawInventoryByProduct(Guid productId)
        {
            try
            {
                var inventory = await _rawInventoryService.GetInventoryByProductAsync(productId);
                return Ok(ApiResponse<IEnumerable<RawInventory>>.SuccessResponse(inventory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<RawInventory>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get available raw quantity for a purchase batch
        /// </summary>
        /// <param name="batchId">Purchase batch unique identifier</param>
        /// <returns>Available raw quantity in kg</returns>
        /// <response code="200">Returns available quantity</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("raw/batch/{batchId}/available")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetAvailableRawQuantity(Guid batchId)
        {
            try
            {
                var quantity = await _rawInventoryService.GetAvailableQuantityAsync(batchId);
                return Ok(ApiResponse<decimal>.SuccessResponse(quantity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get finished inventory by location
        /// </summary>
        /// <param name="locationId">Location unique identifier</param>
        /// <returns>List of finished inventory items at the location</returns>
        /// <response code="200">Returns finished inventory items</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("finished/location/{locationId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinishedInventory>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinishedInventory>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<FinishedInventory>>>> GetFinishedInventoryByLocation(Guid locationId)
        {
            try
            {
                var inventory = await _finishedInventoryService.GetFinishedInventoryByLocationAsync(locationId);
                return Ok(ApiResponse<IEnumerable<FinishedInventory>>.SuccessResponse(inventory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<FinishedInventory>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get finished inventory by product
        /// </summary>
        /// <param name="productId">Product unique identifier</param>
        /// <returns>List of finished inventory items for the product</returns>
        /// <response code="200">Returns finished inventory items</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("finished/product/{productId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinishedInventory>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinishedInventory>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<FinishedInventory>>>> GetFinishedInventoryByProduct(Guid productId)
        {
            try
            {
                var inventory = await _finishedInventoryService.GetFinishedInventoryByProductAsync(productId);
                return Ok(ApiResponse<IEnumerable<FinishedInventory>>.SuccessResponse(inventory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<FinishedInventory>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get available finished stock for sale (FIFO ordered)
        /// </summary>
        /// <param name="productId">Product unique identifier</param>
        /// <returns>List of available finished inventory items ordered by FIFO</returns>
        /// <response code="200">Returns available finished stock</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("finished/product/{productId}/available")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinishedInventory>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FinishedInventory>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<FinishedInventory>>>> GetAvailableFinishedStock(Guid productId)
        {
            try
            {
                var stock = await _finishedInventoryService.GetAvailableStockForSaleAsync(productId);
                return Ok(ApiResponse<IEnumerable<FinishedInventory>>.SuccessResponse(stock));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<FinishedInventory>>.ErrorResponse(ex.Message));
            }
        }
    }
}
