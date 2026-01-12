using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Purchase;
using AgroProcessing.DTOs.Processing;
using AgroProcessing.DTOs.Sales;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers
{
    /// <summary>
    /// Controller for managing operational workflows
    /// </summary>
    public class WorkflowController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IProcessingRunService _processingService;
        private readonly ISalesService _salesService;
        private readonly IMasterDataService _masterDataService;
        private readonly IRawInventoryService _rawInventoryService;
        private readonly IFinishedInventoryService _finishedInventoryService;
        private readonly IPurchasePaymentService _purchasePaymentService;

        public WorkflowController(
            IPurchaseService purchaseService,
            IProcessingRunService processingService,
            ISalesService salesService,
            IMasterDataService masterDataService,
            IRawInventoryService rawInventoryService,
            IFinishedInventoryService finishedInventoryService,
            IPurchasePaymentService purchasePaymentService)
        {
            _purchaseService = purchaseService;
            _processingService = processingService;
            _salesService = salesService;
            _masterDataService = masterDataService;
            _rawInventoryService = rawInventoryService;
            _finishedInventoryService = finishedInventoryService;
            _purchasePaymentService = purchasePaymentService;
        }

        // Dashboard
        public IActionResult Index()
        {
            return View();
        }

        #region Purchase Workflow

        public async Task<IActionResult> Purchases()
        {
            var purchases = await _purchaseService.GetPendingPurchaseBatchesAsync();
            return View(purchases);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePurchase()
        {
            ViewBag.Farmers = await _masterDataService.GetAllFarmersAsync();
            ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchase(CreatePurchaseBatchDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Farmers = await _masterDataService.GetAllFarmersAsync();
                ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
                return View(dto);
            }

            try
            {
                var purchase = await _purchaseService.CreatePurchaseBatchAsync(dto);
                TempData["SuccessMessage"] = "Purchase created successfully!";
                return RedirectToAction(nameof(PurchaseDetails), new { id = purchase.PurchaseBatchId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Farmers = await _masterDataService.GetAllFarmersAsync();
                ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PurchaseDetails(Guid id)
        {
            var purchase = await _purchaseService.GetPurchaseBatchByIdAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            var outstanding = await _purchaseService.GetOutstandingAmountAsync(id);
            var inventoryList = await _rawInventoryService.GetInventoryByProductAsync(purchase.ProductId);
            var inventory = inventoryList?.FirstOrDefault(i => i.PurchaseBatchId == id);
            
            ViewBag.Outstanding = outstanding;
            ViewBag.Inventory = inventory;
            
            return View(purchase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordPurchasePayment(RecordPaymentDto dto)
        {
            try
            {
                await _purchasePaymentService.RecordPaymentAsync(dto);
                TempData["SuccessMessage"] = "Payment recorded successfully!";
                return RedirectToAction(nameof(PurchaseDetails), new { id = dto.PurchaseBatchId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(PurchaseDetails), new { id = dto.PurchaseBatchId });
            }
        }

        #endregion

        #region Processing Workflow

        public async Task<IActionResult> Processing()
        {
            var runs = await _processingService.GetActiveProcessingRunsAsync();
            return View(runs);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProcessingRun()
        {
            ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProcessingRun(CreateProcessingRunDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
                return View(dto);
            }

            try
            {
                var run = await _processingService.CreateProcessingRunAsync(dto);
                TempData["SuccessMessage"] = "Processing run created successfully!";
                return RedirectToAction(nameof(ProcessingDetails), new { id = run.ProcessingRunId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProcessingDetails(Guid id)
        {
            var run = await _processingService.GetProcessingRunByIdAsync(id);
            if (run == null)
            {
                return NotFound();
            }

            var totalCost = await _processingService.CalculateTotalProcessingCostAsync(id);
            ViewBag.TotalCost = totalCost;
            
            return View(run);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProcessingRun(CompleteProcessingRunDto dto)
        {
            try
            {
                await _processingService.CompleteProcessingRunAsync(dto);
                TempData["SuccessMessage"] = "Processing run completed successfully!";
                return RedirectToAction(nameof(ProcessingDetails), new { id = dto.ProcessingRunId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(ProcessingDetails), new { id = dto.ProcessingRunId });
            }
        }

        #endregion

        #region Sales Workflow

        public async Task<IActionResult> Sales()
        {
            var sales = await _salesService.GetOpenSalesAsync();
            return View(sales);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSale()
        {
            ViewBag.Buyers = await _masterDataService.GetAllBuyersAsync();
            ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSale(CreateSaleDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Buyers = await _masterDataService.GetAllBuyersAsync();
                ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
                return View(dto);
            }

            try
            {
                var sale = await _salesService.CreateSaleAsync(dto);
                TempData["SuccessMessage"] = "Sale created successfully!";
                return RedirectToAction(nameof(SaleDetails), new { id = sale.SaleId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Buyers = await _masterDataService.GetAllBuyersAsync();
                ViewBag.Products = await _masterDataService.GetActiveProductsAsync();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SaleDetails(Guid id)
        {
            var sale = await _salesService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            var outstanding = 0m; // TODO: Get from payment service
            ViewBag.Outstanding = outstanding;
            
            return View(sale);
        }

        [HttpPost]
        public async Task<IActionResult> CloseSale(Guid id)
        {
            try
            {
                await _salesService.CloseSaleAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region API Endpoints for Dynamic Data

        [HttpGet]
        public async Task<IActionResult> GetAvailableInventory(Guid productId)
        {
            try
            {
                var inventory = await _rawInventoryService.GetInventoryByProductAsync(productId);
                var availableInventory = inventory.Select(i => new
                {
                    purchaseBatchId = i.PurchaseBatchId,
                    availableWeight = i.Quantity
                });
                return Json(new { success = true, data = availableInventory });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFinishedInventory(Guid productId)
        {
            try
            {
                var inventory = await _finishedInventoryService.GetFinishedInventoryByProductAsync(productId);
                var availableInventory = inventory.Select(i => new
                {
                    batchId = i.FinishedInventoryId,
                    availableWeight = i.Quantity
                });
                return Json(new { success = true, data = availableInventory });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}
