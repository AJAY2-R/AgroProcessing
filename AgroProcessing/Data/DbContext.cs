using Microsoft.EntityFrameworkCore;
using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Farmer> Farmers => Set<Farmer>();
        public DbSet<Worker> Workers => Set<Worker>();
        public DbSet<WorkType> WorkTypes => Set<WorkType>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<PurchaseBatch> PurchaseBatches => Set<PurchaseBatch>();
        public DbSet<PurchasePayment> PurchasePayments => Set<PurchasePayment>();
        public DbSet<RawInventory> RawInventories => Set<RawInventory>();
        public DbSet<ProcessingRun> ProcessingRuns => Set<ProcessingRun>();
        public DbSet<ProcessingRunInput> ProcessingRunInputs => Set<ProcessingRunInput>();
        public DbSet<ProcessingStage> ProcessingStages => Set<ProcessingStage>();
        public DbSet<ProcessingStageWorker> ProcessingStageWorkers => Set<ProcessingStageWorker>();
        public DbSet<ProcessingCost> ProcessingCosts => Set<ProcessingCost>();
        public DbSet<ProcessingOutput> ProcessingOutputs => Set<ProcessingOutput>();
        public DbSet<BatchOutputAllocation> BatchOutputAllocations => Set<BatchOutputAllocation>();
        public DbSet<FinishedInventory> FinishedInventories => Set<FinishedInventory>();
        public DbSet<Buyer> Buyers => Set<Buyer>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();
        public DbSet<SaleBatchAllocation> SaleBatchAllocations => Set<SaleBatchAllocation>();
        public DbSet<SalesPayment> SalesPayments => Set<SalesPayment>();
        public DbSet<Transportation> Transportations => Set<Transportation>();
        public DbSet<WorkerPayment> WorkerPayments => Set<WorkerPayment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
