using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Location management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Master Data - Locations")]
    public class LocationsController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public LocationsController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns>List of all locations organized by type</returns>
        /// <response code="200">Returns the list of locations</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Location>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Location>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Location>>>> GetAllLocations()
        {
            try
            {
                var locations = await _masterDataService.GetAllLocationsAsync();
                return Ok(ApiResponse<IEnumerable<Location>>.SuccessResponse(locations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Location>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Create new location
        /// </summary>
        /// <param name="location">Location details (Raw, Processing, Drying, Finished)</param>
        /// <returns>Created location</returns>
        /// <response code="200">Location created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Location>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Location>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Location>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Location>>> CreateLocation([FromBody] Location location)
        {
            try
            {
                var created = await _masterDataService.CreateLocationAsync(location);
                return Ok(ApiResponse<Location>.SuccessResponse(created, "Location created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Location>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Location>.ErrorResponse(ex.Message));
            }
        }
    }
}
