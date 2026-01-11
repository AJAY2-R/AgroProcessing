using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Worker payment management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Worker Payments")]
    public class WorkerPaymentsController : ControllerBase
    {
        private readonly IWorkerPaymentService _workerPaymentService;

        public WorkerPaymentsController(IWorkerPaymentService workerPaymentService)
        {
            _workerPaymentService = workerPaymentService;
        }

        /// <summary>
        /// Create a new worker payment
        /// </summary>
        /// <param name="dto">Payment details including worker, stage, and amount</param>
        /// <returns>Created worker payment</returns>
        /// <response code="200">Worker payment created successfully</response>
        /// <response code="400">Invalid input or duplicate payment</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<WorkerPayment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WorkerPayment>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<WorkerPayment>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<WorkerPayment>>> CreateWorkerPayment([FromBody] CreateWorkerPaymentDto dto)
        {
            try
            {
                var payment = await _workerPaymentService.CreateWorkerPaymentAsync(
                    dto.WorkerId, dto.ProcessingStageId, dto.Amount);
                return Ok(ApiResponse<WorkerPayment>.SuccessResponse(payment, "Worker payment created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<WorkerPayment>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<WorkerPayment>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<WorkerPayment>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all unpaid worker payments
        /// </summary>
        /// <returns>List of unpaid worker payments</returns>
        /// <response code="200">Returns unpaid worker payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("unpaid")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkerPayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkerPayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkerPayment>>>> GetUnpaidWorkerPayments()
        {
            try
            {
                var payments = await _workerPaymentService.GetUnpaidWorkerPaymentsAsync();
                return Ok(ApiResponse<IEnumerable<WorkerPayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<WorkerPayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all payments for a specific worker
        /// </summary>
        /// <param name="workerId">Worker unique identifier</param>
        /// <returns>List of worker payments</returns>
        /// <response code="200">Returns the list of payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("worker/{workerId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkerPayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkerPayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkerPayment>>>> GetWorkerPaymentsByWorker(Guid workerId)
        {
            try
            {
                var payments = await _workerPaymentService.GetWorkerPaymentsByWorkerAsync(workerId);
                return Ok(ApiResponse<IEnumerable<WorkerPayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<WorkerPayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get total unpaid amount for a worker
        /// </summary>
        /// <param name="workerId">Worker unique identifier</param>
        /// <returns>Total unpaid amount</returns>
        /// <response code="200">Returns the total unpaid amount</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("worker/{workerId}/unpaid-total")]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalUnpaidForWorker(Guid workerId)
        {
            try
            {
                var total = await _workerPaymentService.GetTotalUnpaidForWorkerAsync(workerId);
                return Ok(ApiResponse<decimal>.SuccessResponse(total));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<decimal>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get all payments for a processing stage
        /// </summary>
        /// <param name="stageId">Processing stage unique identifier</param>
        /// <returns>List of worker payments for the stage</returns>
        /// <response code="200">Returns the list of payments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("stage/{stageId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkerPayment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkerPayment>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkerPayment>>>> GetPaymentsByProcessingStage(Guid stageId)
        {
            try
            {
                var payments = await _workerPaymentService.GetPaymentsByProcessingStageAsync(stageId);
                return Ok(ApiResponse<IEnumerable<WorkerPayment>>.SuccessResponse(payments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<WorkerPayment>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Mark a worker payment as paid
        /// </summary>
        /// <param name="id">Payment unique identifier</param>
        /// <param name="paymentDate">Date payment was made</param>
        /// <returns>Result of operation</returns>
        /// <response code="200">Worker payment marked as paid successfully</response>
        /// <response code="404">Worker payment not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}/mark-paid")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> MarkWorkerPaymentAsPaid(Guid id, [FromBody] DateTime paymentDate)
        {
            try
            {
                var result = await _workerPaymentService.MarkWorkerPaymentAsPaidAsync(id, paymentDate);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Worker payment not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(result, "Worker payment marked as paid"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }

    /// <summary>
    /// Data transfer object for creating worker payment
    /// </summary>
    public class CreateWorkerPaymentDto
    {
        /// <summary>
        /// Worker unique identifier
        /// </summary>
        public Guid WorkerId { get; set; }
        
        /// <summary>
        /// Processing stage unique identifier
        /// </summary>
        public Guid ProcessingStageId { get; set; }
        
        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}
