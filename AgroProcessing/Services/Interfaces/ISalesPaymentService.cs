using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Services.Interfaces
{
    public interface ISalesPaymentService
    {
        Task<SalesPayment> RecordSalesPaymentAsync(Guid saleId, decimal amountPaid, DateTime? paymentDate, DateTime dueDate, string paymentMode);
        Task<IEnumerable<SalesPayment>> GetPaymentsBySaleAsync(Guid saleId);
        Task<decimal> GetTotalPaidForSaleAsync(Guid saleId);
        Task<IEnumerable<SalesPayment>> GetOverdueSalesPaymentsAsync();
        Task<bool> MarkSalesPaymentAsPaidAsync(Guid paymentId, DateTime paymentDate);
    }
}
