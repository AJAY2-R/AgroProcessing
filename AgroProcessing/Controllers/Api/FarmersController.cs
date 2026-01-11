using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Farmer management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Master Data - Farmers")]
    public class FarmersController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public FarmersController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// Get all farmers
        /// </summary>
        /// <returns>List of all farmers</returns>
        /// <response code="200">Returns the list of farmers</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Farmer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Farmer>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Farmer>>>> GetAllFarmers()
        {
            try
            {
                var farmers = await _masterDataService.GetAllFarmersAsync();
                return Ok(ApiResponse<IEnumerable<Farmer>>.SuccessResponse(farmers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Farmer>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get farmer by ID
        /// </summary>
        /// <param name="id">Farmer unique identifier</param>
        /// <returns>Farmer details</returns>
        /// <response code="200">Returns the farmer</response>
        /// <response code="404">Farmer not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Farmer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Farmer>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Farmer>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Farmer>>> GetFarmerById(Guid id)
        {
            try
            {
                var farmer = await _masterDataService.GetFarmerByIdAsync(id);
                if (farmer == null)
                    return NotFound(ApiResponse<Farmer>.ErrorResponse("Farmer not found"));

                return Ok(ApiResponse<Farmer>.SuccessResponse(farmer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Farmer>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Create new farmer
        /// </summary>
        /// <param name="farmer">Farmer details</param>
        /// <returns>Created farmer</returns>
        /// <response code="201">Farmer created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Farmer>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Farmer>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Farmer>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Farmer>>> CreateFarmer([FromBody] Farmer farmer)
        {
            try
            {
                var created = await _masterDataService.CreateFarmerAsync(farmer);
                return CreatedAtAction(nameof(GetFarmerById), new { id = created.FarmerId },
                    ApiResponse<Farmer>.SuccessResponse(created, "Farmer created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Farmer>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Farmer>.ErrorResponse(ex.Message));
            }
        }
    }
}
