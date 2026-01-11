using System.Diagnostics;
using AgroProcessing.Models;
using AgroProcessing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroProcessing.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IProcessingRunService _processingService;
        private readonly ISalesService _salesService;
        private readonly IRawInventoryService _rawInventoryService;
        private readonly IFinishedInventoryService _finishedInventoryService;
        private readonly IMasterDataService _masterDataService;

        public HomeController(
            IPurchaseService purchaseService,
            IProcessingRunService processingService,
            ISalesService salesService,
            IRawInventoryService rawInventoryService,
            IFinishedInventoryService finishedInventoryService,
            IMasterDataService masterDataService)
        {
            _purchaseService = purchaseService;
            _processingService = processingService;
            _salesService = salesService;
            _rawInventoryService = rawInventoryService;
            _finishedInventoryService = finishedInventoryService;
            _masterDataService = masterDataService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get quick stats for home page
                var farmers = await _masterDataService.GetAllFarmersAsync();
                var buyers = await _masterDataService.GetAllBuyersAsync();
                var products = await _masterDataService.GetActiveProductsAsync();
                var workers = await _masterDataService.GetActiveWorkersAsync();
                var pendingPurchases = await _purchaseService.GetPendingPurchaseBatchesAsync();
                var openSales = await _salesService.GetOpenSalesAsync();

                ViewBag.TotalFarmers = farmers.Count();
                ViewBag.TotalBuyers = buyers.Count();
                ViewBag.TotalProducts = products.Count();
                ViewBag.TotalWorkers = workers.Count();
                ViewBag.PendingPurchases = pendingPurchases.Count();
                ViewBag.OpenSales = openSales.Count();
                ViewBag.PendingAmount = pendingPurchases.Sum(p => p.TotalAmount);
                ViewBag.SalesAmount = openSales.Sum(s => s.TotalAmount);

                return View();
            }
            catch (Exception)
            {
                // Return view with default values on error
                ViewBag.TotalFarmers = 0;
                ViewBag.TotalBuyers = 0;
                ViewBag.TotalProducts = 0;
                ViewBag.TotalWorkers = 0;
                ViewBag.PendingPurchases = 0;
                ViewBag.OpenSales = 0;
                ViewBag.PendingAmount = 0;
                ViewBag.SalesAmount = 0;
                
                return View();
            }
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Get all data for dashboard - using AsNoTracking for read-only queries
                var pendingPurchases = await _purchaseService.GetPendingPurchaseBatchesAsync();
                var activeProcessingRuns = await _processingService.GetActiveProcessingRunsAsync();
                var openSales = await _salesService.GetOpenSalesAsync();
                var products = await _masterDataService.GetActiveProductsAsync();
                var workers = await _masterDataService.GetActiveWorkersAsync();

                // Create dashboard view model
                var model = new DashboardViewModel
                {
                    TotalPurchases = pendingPurchases.Count(),
                    ActiveProcessingRuns = activeProcessingRuns.Count(),
                    TotalSalesAmount = openSales.Sum(s => s.TotalAmount),
                    ActiveWorkers = workers.Count(),
                    RecentPurchases = pendingPurchases.Take(5).ToList(),
                    ActiveProcessing = activeProcessingRuns.Take(5).ToList(),
                    RecentSales = openSales.Take(5).ToList(),
                    Products = products.ToList()
                };

                // Get inventory data
                foreach (var product in products)
                {
                    var rawInventory = await _rawInventoryService.GetInventoryByProductAsync(product.ProductId);
                    var finishedInventory = await _finishedInventoryService.GetFinishedInventoryByProductAsync(product.ProductId);

                    model.InventoryData.Add(new InventoryOverview
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        RawStock = rawInventory.Sum(i => i.Quantity),
                        FinishedStock = finishedInventory.Sum(i => i.Quantity)
                    });
                }

                return View(model);
            }
            catch (Exception)
            {
                // Return empty model on error
                return View(new DashboardViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
