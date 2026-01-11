using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Business intelligence and reporting endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Get comprehensive profit report for a purchase batch
        /// </summary>
        /// <param name="batchId">Purchase batch unique identifier</param>
        /// <returns>Detailed profit breakdown including costs and revenue</returns>
        /// <response code="200">Returns the profit report</response>
        /// <response code="400">Batch not found or not completed</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("batch-profit/{batchId}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetBatchProfitReport(Guid batchId)
        {
            try
            {
                var report = await _reportService.GetBatchProfitReportAsync(batchId);
                return Ok(ApiResponse<object>.SuccessResponse(report));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get yield analysis report for a product
        /// </summary>
        /// <param name="productId">Product unique identifier</param>
        /// <returns>Yield statistics including average, min, max percentages</returns>
        /// <response code="200">Returns the yield analysis</response>
        /// <response code="400">Product not found or no processing data</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("yield-analysis/{productId}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetYieldAnalysisReport(Guid productId)
        {
            try
            {
                var report = await _reportService.GetYieldAnalysisReportAsync(productId);
                return Ok(ApiResponse<object>.SuccessResponse(report));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get outstanding payments report (purchases and sales)
        /// </summary>
        /// <returns>Summary of all outstanding payments</returns>
        /// <response code="200">Returns the outstanding payments report</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("outstanding-payments")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetOutstandingPaymentsReport()
        {
            try
            {
                var report = await _reportService.GetOutstandingPaymentsReportAsync();
                return Ok(ApiResponse<object>.SuccessResponse(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get inventory valuation report
        /// </summary>
        /// <returns>Current value of raw and finished inventory</returns>
        /// <response code="200">Returns the inventory valuation</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("inventory-valuation")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetInventoryValuationReport()
        {
            try
            {
                var report = await _reportService.GetInventoryValuationReportAsync();
                return Ok(ApiResponse<object>.SuccessResponse(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get worker payment summary report
        /// </summary>
        /// <param name="workerId">Optional worker unique identifier for specific worker report</param>
        /// <returns>Summary of worker payments (all workers or specific worker)</returns>
        /// <response code="200">Returns the worker payment summary</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("worker-payment-summary")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetWorkerPaymentSummary([FromQuery] Guid? workerId = null)
        {
            try
            {
                var report = await _reportService.GetWorkerPaymentSummaryAsync(workerId);
                return Ok(ApiResponse<object>.SuccessResponse(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get processing cost analysis for a run
        /// </summary>
        /// <param name="runId">Processing run unique identifier</param>
        /// <returns>Detailed cost breakdown including workers, materials, and other costs</returns>
        /// <response code="200">Returns the cost analysis</response>
        /// <response code="400">Run not found or not completed</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("processing-cost-analysis/{runId}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetProcessingCostAnalysis(Guid runId)
        {
            try
            {
                var report = await _reportService.GetProcessingCostAnalysisAsync(runId);
                return Ok(ApiResponse<object>.SuccessResponse(report));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }
    }
}
