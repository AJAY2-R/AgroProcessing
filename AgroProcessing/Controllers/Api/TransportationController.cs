using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Transportation cost management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Transportation")]
    public class TransportationController : ControllerBase
    {
        private readonly ITransportationService _transportationService;

        public TransportationController(ITransportationService transportationService)
        {
            _transportationService = transportationService;
        }

        /// <summary>
        /// Record a new transportation cost
        /// </summary>
        /// <param name="dto">Transportation details including from/to locations and cost</param>
        /// <returns>Created transportation record</returns>
        /// <response code="200">Transportation recorded successfully</response>
        /// <response code="400">Invalid input or locations are the same</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Transportation>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Transportation>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Transportation>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Transportation>>> RecordTransportation([FromBody] RecordTransportationDto dto)
        {
            try
            {
                var transportation = await _transportationService.RecordTransportationAsync(
                    dto.RelatedType, dto.RelatedId, dto.FromLocationId, dto.ToLocationId, dto.Cost);
                return Ok(ApiResponse<Transportation>.SuccessResponse(transportation, "Transportation recorded successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Transportation>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<Transportation>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Transportation>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all transportation records for a specific type and ID
        /// </summary>
        /// <param name="relatedType">Type (Purchase, Processing, Sale)</param>
        /// <param name="relatedId">Related record unique identifier</param>
        /// <returns>List of transportation records</returns>
        /// <response code="200">Returns the list of transportations</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{relatedType}/{relatedId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Transportation>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Transportation>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Transportation>>>> GetTransportationByType(string relatedType, Guid relatedId)
        {
            try
            {
                var transportations = await _transportationService.GetTransportationByTypeAsync(relatedType, relatedId);
                return Ok(ApiResponse<IEnumerable<Transportation>>.SuccessResponse(transportations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Transportation>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get total transportation cost for a specific type and ID
        /// </summary>
        /// <param name="relatedType">Type (Purchase, Processing, Sale)</param>
        /// <param name="relatedId">Related record unique identifier</param>
        /// <returns>Total transportation cost</returns>
        /// <response code="200">Returns the total cost</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{relatedType}/{relatedId}/total-cost")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalTransportationCost(string relatedType, Guid relatedId)
        {
            try
            {
                var total = await _transportationService.GetTotalTransportationCostAsync(relatedType, relatedId);
                return Ok(ApiResponse<decimal>.SuccessResponse(total));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all unpaid transportation records
        /// </summary>
        /// <returns>List of unpaid transportation records</returns>
        /// <response code="200">Returns unpaid transportations</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("unpaid")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Transportation>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Transportation>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Transportation>>>> GetUnpaidTransportations()
        {
            try
            {
                var transportations = await _transportationService.GetUnpaidTransportationsAsync();
                return Ok(ApiResponse<IEnumerable<Transportation>>.SuccessResponse(transportations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Transportation>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Mark a transportation record as paid
        /// </summary>
        /// <param name="id">Transportation unique identifier</param>
        /// <returns>Result of operation</returns>
        /// <response code="200">Transportation marked as paid successfully</response>
        /// <response code="404">Transportation record not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}/mark-paid")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> MarkTransportationAsPaid(Guid id)
        {
            try
            {
                var result = await _transportationService.MarkTransportationAsPaidAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Transportation record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(result, "Transportation marked as paid"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }

    /// <summary>
    /// Data transfer object for recording transportation
    /// </summary>
    public class RecordTransportationDto
    {
        /// <summary>
        /// Type of related record (Purchase, Processing, Sale)
        /// </summary>
        public string RelatedType { get; set; } = null!;
        
        /// <summary>
        /// Unique identifier of the related record
        /// </summary>
        public Guid RelatedId { get; set; }
        
        /// <summary>
        /// Origin location unique identifier
        /// </summary>
        public Guid FromLocationId { get; set; }
        
        /// <summary>
        /// Destination location unique identifier
        /// </summary>
        public Guid ToLocationId { get; set; }
        
        /// <summary>
        /// Transportation cost amount
        /// </summary>
        public decimal Cost { get; set; }
    }
}
