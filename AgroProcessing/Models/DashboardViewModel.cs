using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Models
{
    public class DashboardViewModel
    {
        public int TotalPurchases { get; set; }
        public int ActiveProcessingRuns { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public int ActiveWorkers { get; set; }

        public List<PurchaseBatch> RecentPurchases { get; set; } = new();
        public List<ProcessingRun> ActiveProcessing { get; set; } = new();
        public List<Sale> RecentSales { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<InventoryOverview> InventoryData { get; set; } = new();
    }

    public class InventoryOverview
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal RawStock { get; set; }
        public decimal FinishedStock { get; set; }
        public string Status => RawStock < 500 ? "Low Stock" : "In Stock";
    }
}
