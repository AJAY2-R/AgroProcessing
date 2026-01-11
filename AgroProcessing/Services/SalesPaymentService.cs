using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class SalesPaymentService : ISalesPaymentService
    {
        private readonly AppDbContext _context;

        public SalesPaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SalesPayment> RecordSalesPaymentAsync(Guid saleId, decimal amountPaid, DateTime? paymentDate, DateTime dueDate, string paymentMode)
        {
            if (amountPaid <= 0)
                throw new ArgumentException("Payment amount must be greater than zero");

            var sale = await _context.Sales.FindAsync(saleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            var totalPaid = await GetTotalPaidForSaleAsync(saleId);
            var outstanding = sale.TotalAmount - totalPaid;

            if (amountPaid > outstanding)
                throw new ArgumentException($"Payment amount ({amountPaid}) exceeds outstanding amount ({outstanding})");

            var payment = new SalesPayment
            {
                SalesPaymentId = Guid.NewGuid(),
                SaleId = saleId,
                AmountPaid = amountPaid,
                PaymentDate = paymentDate,
                DueDate = dueDate,
                PaymentMode = paymentMode,
                PaymentStatus = paymentDate.HasValue ? "Paid" : "Pending"
            };

            _context.SalesPayments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<IEnumerable<SalesPayment>> GetPaymentsBySaleAsync(Guid saleId)
        {
            return await _context.SalesPayments
                .AsNoTracking()
                .Where(sp => sp.SaleId == saleId)
                .OrderBy(sp => sp.DueDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaidForSaleAsync(Guid saleId)
        {
            return await _context.SalesPayments
                .AsNoTracking()
                .Where(sp => sp.SaleId == saleId)
                .SumAsync(sp => sp.AmountPaid);
        }

        public async Task<IEnumerable<SalesPayment>> GetOverdueSalesPaymentsAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.SalesPayments
                .AsNoTracking()
                .Where(sp => sp.PaymentStatus == "Pending" && sp.DueDate < today)
                .OrderBy(sp => sp.DueDate)
                .ToListAsync();
        }

        public async Task<bool> MarkSalesPaymentAsPaidAsync(Guid paymentId, DateTime paymentDate)
        {
            var payment = await _context.SalesPayments.FindAsync(paymentId);
            if (payment == null)
                return false;

            payment.PaymentDate = paymentDate;
            payment.PaymentStatus = "Paid";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
