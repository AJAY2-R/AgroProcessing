using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers
{
    /// <summary>
    /// Admin controller for managing master data
    /// </summary>
    public class AdminController : Controller
    {
        private readonly IMasterDataService _masterDataService;

        public AdminController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        // Dashboard
        public IActionResult Index()
        {
            return View();
        }

        #region Products

        public async Task<IActionResult> Products()
        {
            var products = await _masterDataService.GetActiveProductsAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                await _masterDataService.CreateProductAsync(product);
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Products));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            var product = await _masterDataService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleProductStatus(Guid id)
        {
            try
            {
                await _masterDataService.ToggleProductStatusAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Farmers

        public async Task<IActionResult> Farmers()
        {
            var farmers = await _masterDataService.GetAllFarmersAsync();
            return View(farmers);
        }

        [HttpGet]
        public IActionResult CreateFarmer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFarmer(Farmer farmer)
        {
            if (!ModelState.IsValid)
            {
                return View(farmer);
            }

            try
            {
                await _masterDataService.CreateFarmerAsync(farmer);
                TempData["SuccessMessage"] = "Farmer created successfully!";
                return RedirectToAction(nameof(Farmers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(farmer);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFarmer(Guid id)
        {
            var farmer = await _masterDataService.GetFarmerByIdAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        #endregion

        #region Workers

        public async Task<IActionResult> Workers()
        {
            var workers = await _masterDataService.GetActiveWorkersAsync();
            return View(workers);
        }

        [HttpGet]
        public IActionResult CreateWorker()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWorker(Worker worker)
        {
            if (!ModelState.IsValid)
            {
                return View(worker);
            }

            try
            {
                await _masterDataService.CreateWorkerAsync(worker);
                TempData["SuccessMessage"] = "Worker created successfully!";
                return RedirectToAction(nameof(Workers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(worker);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditWorker(Guid id)
        {
            var worker = await _masterDataService.GetWorkerByIdAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWorkerStatus(Guid id)
        {
            try
            {
                await _masterDataService.ToggleWorkerStatusAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Buyers

        public async Task<IActionResult> Buyers()
        {
            var buyers = await _masterDataService.GetAllBuyersAsync();
            return View(buyers);
        }

        [HttpGet]
        public IActionResult CreateBuyer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBuyer(Buyer buyer)
        {
            if (!ModelState.IsValid)
            {
                return View(buyer);
            }

            try
            {
                await _masterDataService.CreateBuyerAsync(buyer);
                TempData["SuccessMessage"] = "Buyer created successfully!";
                return RedirectToAction(nameof(Buyers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(buyer);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBuyer(Guid id)
        {
            var buyer = await _masterDataService.GetBuyerByIdAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            return View(buyer);
        }

        #endregion

        #region Locations

        public async Task<IActionResult> Locations()
        {
            var locations = await _masterDataService.GetAllLocationsAsync();
            return View(locations);
        }

        [HttpGet]
        public IActionResult CreateLocation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return View(location);
            }

            try
            {
                await _masterDataService.CreateLocationAsync(location);
                TempData["SuccessMessage"] = "Location created successfully!";
                return RedirectToAction(nameof(Locations));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(location);
            }
        }

        #endregion
    }
}
