using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class MasterDataService : IMasterDataService
    {
        private readonly AppDbContext _context;

        public MasterDataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Worker>> GetActiveWorkersAsync()
        {
            return await _context.Workers
                .Where(w => w.IsActive)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkType>> GetActiveWorkTypesAsync()
        {
            return await _context.WorkTypes
                .Where(wt => wt.IsActive)
                .OrderBy(wt => wt.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .OrderBy(l => l.LocationType)
                .ThenBy(l => l.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            return await _context.Farmers
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Buyer>> GetAllBuyersAsync()
        {
            return await _context.Buyers
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<Worker?> GetWorkerByIdAsync(Guid workerId)
        {
            return await _context.Workers.FindAsync(workerId);
        }

        public async Task<Farmer?> GetFarmerByIdAsync(Guid farmerId)
        {
            return await _context.Farmers.FindAsync(farmerId);
        }

        public async Task<Buyer?> GetBuyerByIdAsync(Guid buyerId)
        {
            return await _context.Buyers.FindAsync(buyerId);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required");

            if (product.ExpectedYieldPercent <= 0 || product.ExpectedYieldPercent > 100)
                throw new ArgumentException("Expected yield percent must be between 0 and 100");

            product.ProductId = Guid.NewGuid();
            product.IsActive = true;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            if (string.IsNullOrWhiteSpace(worker.Name))
                throw new ArgumentException("Worker name is required");

            if (worker.DefaultRate < 0)
                throw new ArgumentException("Default rate cannot be negative");

            worker.WorkerId = Guid.NewGuid();
            worker.IsActive = true;

            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return worker;
        }

        public async Task<Farmer> CreateFarmerAsync(Farmer farmer)
        {
            if (string.IsNullOrWhiteSpace(farmer.Name))
                throw new ArgumentException("Farmer name is required");

            farmer.FarmerId = Guid.NewGuid();

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();

            return farmer;
        }

        public async Task<Buyer> CreateBuyerAsync(Buyer buyer)
        {
            if (string.IsNullOrWhiteSpace(buyer.Name))
                throw new ArgumentException("Buyer name is required");

            if (buyer.CreditLimit < 0)
                throw new ArgumentException("Credit limit cannot be negative");

            buyer.BuyerId = Guid.NewGuid();

            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync();

            return buyer;
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            if (string.IsNullOrWhiteSpace(location.Name))
                throw new ArgumentException("Location name is required");

            if (string.IsNullOrWhiteSpace(location.LocationType))
                throw new ArgumentException("Location type is required");

            location.LocationId = Guid.NewGuid();

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return location;
        }

        public async Task<bool> ToggleProductStatusAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleWorkerStatusAsync(Guid workerId)
        {
            var worker = await _context.Workers.FindAsync(workerId);
            if (worker == null)
                return false;

            worker.IsActive = !worker.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
