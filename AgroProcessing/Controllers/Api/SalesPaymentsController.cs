using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Sales payment management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Sales Payments")]
    public class SalesPaymentsController : ControllerBase
    {
        private readonly ISalesPaymentService _paymentService;

        public SalesPaymentsController(ISalesPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Record a new sales payment
        /// </summary>
        /// <param name="dto">Payment details including amount and dates</param>
        /// <returns>Created payment record</returns>
        /// <response code="200">Payment recorded successfully</response>
        /// <response code="400">Invalid input or payment exceeds outstanding amount</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SalesPayment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SalesPayment>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SalesPayment>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<SalesPayment>>> RecordSalesPayment([FromBody] RecordSalesPaymentDto dto)
        {
            try
            {
                var payment = await _paymentService.RecordSalesPaymentAsync(
                    dto.SaleId, dto.AmountPaid, dto.PaymentDate, dto.DueDate, dto.PaymentMode);
                return Ok(ApiResponse<SalesPayment>.SuccessResponse(payment, "Payment recorded successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<SalesPayment>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<SalesPayment>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SalesPayment>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all payments for a specific sale
        /// </summary>
        /// <param name="saleId">Sale unique identifier</param>
        /// <returns>List of payments ordered by due date</returns>
        /// <response code="200">Returns the list of payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("sale/{saleId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SalesPayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SalesPayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<SalesPayment>>>> GetPaymentsBySale(Guid saleId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsBySaleAsync(saleId);
                return Ok(ApiResponse<IEnumerable<SalesPayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<SalesPayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get total paid amount for a sale
        /// </summary>
        /// <param name="saleId">Sale unique identifier</param>
        /// <returns>Total amount paid</returns>
        /// <response code="200">Returns the total paid amount</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("sale/{saleId}/total")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalPaidForSale(Guid saleId)
        {
            try
            {
                var total = await _paymentService.GetTotalPaidForSaleAsync(saleId);
                return Ok(ApiResponse<decimal>.SuccessResponse(total));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all overdue sales payments
        /// </summary>
        /// <returns>List of overdue payments</returns>
        /// <response code="200">Returns overdue payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("overdue")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SalesPayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SalesPayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<SalesPayment>>>> GetOverduePayments()
        {
            try
            {
                var payments = await _paymentService.GetOverdueSalesPaymentsAsync();
                return Ok(ApiResponse<IEnumerable<SalesPayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<SalesPayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Mark a sales payment as paid
        /// </summary>
        /// <param name="id">Payment unique identifier</param>
        /// <param name="paymentDate">Date payment was made</param>
        /// <returns>Result of operation</returns>
        /// <response code="200">Payment marked as paid successfully</response>
        /// <response code="404">Payment not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}/mark-paid")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> MarkPaymentAsPaid(Guid id, [FromBody] DateTime paymentDate)
        {
            try
            {
                var result = await _paymentService.MarkSalesPaymentAsPaidAsync(id, paymentDate);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Payment not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(result, "Payment marked as paid"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }

    /// <summary>
    /// Data transfer object for recording sales payment
    /// </summary>
    public class RecordSalesPaymentDto
    {
        /// <summary>
        /// Sale unique identifier
        /// </summary>
        public Guid SaleId { get; set; }
        
        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal AmountPaid { get; set; }
        
        /// <summary>
        /// Date payment was made (optional for future payments)
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        
        /// <summary>
        /// Payment due date
        /// </summary>
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// Payment mode (Cash, Bank Transfer, Check, etc.)
        /// </summary>
        public string PaymentMode { get; set; } = null!;
    }
}
