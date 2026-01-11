using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.DTOs.Purchase;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Purchase payment management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Purchase Payments")]
    public class PurchasePaymentsController : ControllerBase
    {
        private readonly IPurchasePaymentService _paymentService;

        public PurchasePaymentsController(IPurchasePaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Record a new purchase payment
        /// </summary>
        /// <param name="dto">Payment details</param>
        /// <returns>Created payment record</returns>
        /// <response code="200">Payment recorded successfully</response>
        /// <response code="400">Invalid input or payment exceeds outstanding amount</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PurchasePayment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PurchasePayment>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PurchasePayment>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PurchasePayment>>> RecordPayment([FromBody] RecordPaymentDto dto)
        {
            try
            {
                var payment = await _paymentService.RecordPaymentAsync(dto);
                return Ok(ApiResponse<PurchasePayment>.SuccessResponse(payment, "Payment recorded successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<PurchasePayment>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<PurchasePayment>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PurchasePayment>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all payments for a specific purchase batch
        /// </summary>
        /// <param name="batchId">Purchase batch unique identifier</param>
        /// <returns>List of payments ordered by due date</returns>
        /// <response code="200">Returns the list of payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("batch/{batchId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchasePayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchasePayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PurchasePayment>>>> GetPaymentsByBatch(Guid batchId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByBatchAsync(batchId);
                return Ok(ApiResponse<IEnumerable<PurchasePayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PurchasePayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get total paid amount for a purchase batch
        /// </summary>
        /// <param name="batchId">Purchase batch unique identifier</param>
        /// <returns>Total amount paid</returns>
        /// <response code="200">Returns the total paid amount</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("batch/{batchId}/total")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalPaid(Guid batchId)
        {
            try
            {
                var total = await _paymentService.GetTotalPaidAsync(batchId);
                return Ok(ApiResponse<decimal>.SuccessResponse(total));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all overdue payments
        /// </summary>
        /// <returns>List of overdue payments</returns>
        /// <response code="200">Returns overdue payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("overdue")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchasePayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PurchasePayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PurchasePayment>>>> GetOverduePayments()
        {
            try
            {
                var payments = await _paymentService.GetOverduePaymentsAsync();
                return Ok(ApiResponse<IEnumerable<PurchasePayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PurchasePayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Mark a payment as paid
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
                var result = await _paymentService.MarkPaymentAsPaidAsync(id, paymentDate);
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
}
