using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Services.Interfaces
{
    public interface IMasterDataService
    {
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Worker>> GetActiveWorkersAsync();
        Task<IEnumerable<WorkType>> GetActiveWorkTypesAsync();
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<IEnumerable<Farmer>> GetAllFarmersAsync();
        Task<IEnumerable<Buyer>> GetAllBuyersAsync();
        
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Worker?> GetWorkerByIdAsync(Guid workerId);
        Task<Farmer?> GetFarmerByIdAsync(Guid farmerId);
        Task<Buyer?> GetBuyerByIdAsync(Guid buyerId);
        
        Task<Product> CreateProductAsync(Product product);
        Task<Worker> CreateWorkerAsync(Worker worker);
        Task<Farmer> CreateFarmerAsync(Farmer farmer);
        Task<Buyer> CreateBuyerAsync(Buyer buyer);
        Task<Location> CreateLocationAsync(Location location);
        
        Task<bool> ToggleProductStatusAsync(Guid productId);
        Task<bool> ToggleWorkerStatusAsync(Guid workerId);
    }
}
