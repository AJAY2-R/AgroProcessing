using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.DTOs.Sales;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Sales management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Sales")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Create a new sale with FIFO inventory allocation
        /// </summary>
        /// <param name="dto">Sale details with items and buyer information</param>
        /// <returns>Created sale with allocated inventory batches</returns>
        /// <response code="201">Sale created successfully</response>
        /// <response code="400">Invalid input, insufficient stock, or credit limit exceeded</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Sale>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Sale>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Sale>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Sale>>> CreateSale([FromBody] CreateSaleDto dto)
        {
            try
            {
                var sale = await _salesService.CreateSaleAsync(dto);
                return CreatedAtAction(nameof(GetSaleById), new { id = sale.SaleId },
                    ApiResponse<Sale>.SuccessResponse(sale, "Sale created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Sale>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<Sale>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Sale>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get sale by ID
        /// </summary>
        /// <param name="id">Sale unique identifier</param>
        /// <returns>Sale with items and payment history</returns>
        /// <response code="200">Returns the sale</response>
        /// <response code="404">Sale not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Sale>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Sale>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Sale>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Sale>>> GetSaleById(Guid id)
        {
            try
            {
                var sale = await _salesService.GetSaleByIdAsync(id);
                if (sale == null)
                    return NotFound(ApiResponse<Sale>.ErrorResponse("Sale not found"));

                return Ok(ApiResponse<Sale>.SuccessResponse(sale));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Sale>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all sales for a specific buyer
        /// </summary>
        /// <param name="buyerId">Buyer unique identifier</param>
        /// <returns>List of sales</returns>
        /// <response code="200">Returns the list of sales</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("buyer/{buyerId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Sale>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Sale>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Sale>>>> GetSalesByBuyer(Guid buyerId)
        {
            try
            {
                var sales = await _salesService.GetSalesByBuyerAsync(buyerId);
                return Ok(ApiResponse<IEnumerable<Sale>>.SuccessResponse(sales));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Sale>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all open (not fully paid) sales
        /// </summary>
        /// <returns>List of open sales</returns>
        /// <response code="200">Returns open sales</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("open")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Sale>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Sale>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Sale>>>> GetOpenSales()
        {
            try
            {
                var sales = await _salesService.GetOpenSalesAsync();
                return Ok(ApiResponse<IEnumerable<Sale>>.SuccessResponse(sales));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Sale>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Close a sale (mark as finalized)
        /// </summary>
        /// <param name="id">Sale unique identifier</param>
        /// <returns>Result of operation</returns>
        /// <response code="200">Sale closed successfully</response>
        /// <response code="404">Sale not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}/close")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> CloseSale(Guid id)
        {
            try
            {
                var result = await _salesService.CloseSaleAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Sale not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(result, "Sale closed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get outstanding amount for a buyer
        /// </summary>
        /// <param name="buyerId">Buyer unique identifier</param>
        /// <returns>Total outstanding amount across all open sales</returns>
        /// <response code="200">Returns the outstanding amount</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("buyer/{buyerId}/outstanding")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetBuyerOutstanding(Guid buyerId)
        {
            try
            {
                var outstanding = await _salesService.GetBuyerOutstandingAsync(buyerId);
                return Ok(ApiResponse<decimal>.SuccessResponse(outstanding));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Validate if buyer can make a purchase within credit limit
        /// </summary>
        /// <param name="buyerId">Buyer unique identifier</param>
        /// <param name="amount">Proposed sale amount</param>
        /// <returns>Boolean indicating if purchase is within credit limit</returns>
        /// <response code="200">Returns validation result</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("buyer/{buyerId}/validate-credit")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> ValidateBuyerCreditLimit(Guid buyerId, [FromBody] decimal amount)
        {
            try
            {
                var canProceed = await _salesService.ValidateBuyerCreditLimitAsync(buyerId, amount);
                return Ok(ApiResponse<bool>.SuccessResponse(canProceed));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }
}
