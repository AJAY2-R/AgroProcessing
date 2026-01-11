using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Buyer management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Master Data - Buyers")]
    public class BuyersController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public BuyersController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// Get all buyers
        /// </summary>
        /// <returns>List of all buyers</returns>
        /// <response code="200">Returns the list of buyers</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Buyer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Buyer>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Buyer>>>> GetAllBuyers()
        {
            try
            {
                var buyers = await _masterDataService.GetAllBuyersAsync();
                return Ok(ApiResponse<IEnumerable<Buyer>>.SuccessResponse(buyers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Buyer>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get buyer by ID
        /// </summary>
        /// <param name="id">Buyer unique identifier</param>
        /// <returns>Buyer details</returns>
        /// <response code="200">Returns the buyer</response>
        /// <response code="404">Buyer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Buyer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Buyer>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Buyer>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Buyer>>> GetBuyerById(Guid id)
        {
            try
            {
                var buyer = await _masterDataService.GetBuyerByIdAsync(id);
                if (buyer == null)
                    return NotFound(ApiResponse<Buyer>.ErrorResponse("Buyer not found"));

                return Ok(ApiResponse<Buyer>.SuccessResponse(buyer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Buyer>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Create new buyer
        /// </summary>
        /// <param name="buyer">Buyer details</param>
        /// <returns>Created buyer</returns>
        /// <response code="201">Buyer created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Buyer>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Buyer>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Buyer>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Buyer>>> CreateBuyer([FromBody] Buyer buyer)
        {
            try
            {
                var created = await _masterDataService.CreateBuyerAsync(buyer);
                return CreatedAtAction(nameof(GetBuyerById), new { id = created.BuyerId },
                    ApiResponse<Buyer>.SuccessResponse(created, "Buyer created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Buyer>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Buyer>.ErrorResponse(ex.Message));
            }
        }
    }
}
