using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Sales;

namespace AgroProcessing.Services.Interfaces
{
    public interface ISalesService
    {
        Task<Sale> CreateSaleAsync(CreateSaleDto dto);
        Task<Sale?> GetSaleByIdAsync(Guid saleId);
        Task<IEnumerable<Sale>> GetSalesByBuyerAsync(Guid buyerId);
        Task<IEnumerable<Sale>> GetOpenSalesAsync();
        Task<bool> CloseSaleAsync(Guid saleId);
        Task<bool> ValidateBuyerCreditLimitAsync(Guid buyerId, decimal saleAmount);
        Task<decimal> GetBuyerOutstandingAsync(Guid buyerId);
    }
}
