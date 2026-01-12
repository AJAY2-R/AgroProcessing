using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers
{
    /// <summary>
    /// Controller for managing inventory views and operations
    /// </summary>
    public class InventoryController : Controller
    {
        private readonly IRawInventoryService _rawInventoryService;
        private readonly IFinishedInventoryService _finishedInventoryService;
        private readonly IMasterDataService _masterDataService;

        public InventoryController(
            IRawInventoryService rawInventoryService,
            IFinishedInventoryService finishedInventoryService,
            IMasterDataService masterDataService)
        {
            _rawInventoryService = rawInventoryService;
            _finishedInventoryService = finishedInventoryService;
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// Raw inventory dashboard
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var locations = await _masterDataService.GetAllLocationsAsync();
            var products = await _masterDataService.GetActiveProductsAsync();
            
            ViewBag.Locations = locations;
            ViewBag.Products = products;
            
            return View();
        }

        /// <summary>
        /// View raw inventory by location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RawInventoryByLocation(Guid locationId)
        {
            var inventory = await _rawInventoryService.GetInventoryByLocationAsync(locationId);
            var location = (await _masterDataService.GetAllLocationsAsync())
                .FirstOrDefault(l => l.LocationId == locationId);
            
            if (location == null)
            {
                return NotFound();
            }

            ViewBag.Location = location;
            return View(inventory);
        }

        /// <summary>
        /// View raw inventory by product
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RawInventoryByProduct(Guid productId)
        {
            var inventory = await _rawInventoryService.GetInventoryByProductAsync(productId);
            var product = (await _masterDataService.GetActiveProductsAsync())
                .FirstOrDefault(p => p.ProductId == productId);
            
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;
            return View(inventory);
        }

        /// <summary>
        /// View detailed raw inventory for a specific batch
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RawInventoryDetails(Guid purchaseBatchId)
        {
            var availableQuantity = await _rawInventoryService.GetAvailableQuantityAsync(purchaseBatchId);
            var inventories = (await _rawInventoryService.GetInventoryByProductAsync(Guid.Empty))
                .Where(i => i.PurchaseBatchId == purchaseBatchId)
                .ToList();
            
            if (!inventories.Any())
            {
                return NotFound();
            }

            ViewBag.AvailableQuantity = availableQuantity;
            ViewBag.PurchaseBatchId = purchaseBatchId;
            
            return View(inventories);
        }

        /// <summary>
        /// Get all raw inventory with detailed information
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AllRawInventory()
        {
            var inventory = await _rawInventoryService.GetAllInventoryAsync();
            return View(inventory);
        }

        /// <summary>
        /// Finished inventory dashboard
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FinishedInventory()
        {
            var locations = await _masterDataService.GetAllLocationsAsync();
            var products = await _masterDataService.GetActiveProductsAsync();
            
            ViewBag.Locations = locations;
            ViewBag.Products = products;
            
            return View();
        }

        /// <summary>
        /// View finished inventory by location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FinishedInventoryByLocation(Guid locationId)
        {
            var inventory = await _finishedInventoryService.GetFinishedInventoryByLocationAsync(locationId);
            var location = (await _masterDataService.GetAllLocationsAsync())
                .FirstOrDefault(l => l.LocationId == locationId);
            
            if (location == null)
            {
                return NotFound();
            }

            ViewBag.Location = location;
            return View(inventory);
        }

        /// <summary>
        /// View finished inventory by product
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FinishedInventoryByProduct(Guid productId)
        {
            var inventory = await _finishedInventoryService.GetFinishedInventoryByProductAsync(productId);
            var product = (await _masterDataService.GetActiveProductsAsync())
                .FirstOrDefault(p => p.ProductId == productId);
            
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;
            return View(inventory);
        }

        /// <summary>
        /// Get inventory summary statistics
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInventorySummary()
        {
            try
            {
                var summary = await _rawInventoryService.GetInventorySummaryAsync();
                return Json(new { success = true, data = summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
