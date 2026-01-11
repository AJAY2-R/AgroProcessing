using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Common;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers.Api
{
    /// <summary>
    /// Product management endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Master Data - Products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public ProductsController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// Get all active products
        /// </summary>
        /// <returns>List of active products</returns>
        /// <response code="200">Returns the list of active products</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAllProducts()
        {
            try
            {
                var products = await _masterDataService.GetActiveProductsAsync();
                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Product>>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="id">Product unique identifier</param>
        /// <returns>Product details</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Product>>> GetProductById(Guid id)
        {
            try
            {
                var product = await _masterDataService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound(ApiResponse<Product>.ErrorResponse("Product not found"));

                return Ok(ApiResponse<Product>.SuccessResponse(product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Product>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="product">Product details</param>
        /// <returns>Created product</returns>
        /// <response code="201">Product created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Product>>> CreateProduct([FromBody] Product product)
        {
            try
            {
                var created = await _masterDataService.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = created.ProductId },
                    ApiResponse<Product>.SuccessResponse(created, "Product created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Product>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Product>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Toggle product active status
        /// </summary>
        /// <param name="id">Product unique identifier</param>
        /// <returns>Result of toggle operation</returns>
        /// <response code="200">Status toggled successfully</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}/toggle-status")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> ToggleProductStatus(Guid id)
        {
            try
            {
                var result = await _masterDataService.ToggleProductStatusAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Product not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(result, "Product status toggled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(ex.Message));
            }
        }
    }
}
