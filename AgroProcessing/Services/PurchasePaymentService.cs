using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.DTOs.Purchase;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class PurchasePaymentService : IPurchasePaymentService
    {
        private readonly AppDbContext _context;

        public PurchasePaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PurchasePayment> RecordPaymentAsync(RecordPaymentDto dto)
        {
            if (dto.AmountPaid <= 0)
                throw new ArgumentException("Payment amount must be greater than zero");

            var batch = await _context.PurchaseBatches
                .Include(pb => pb.Payments)
                .FirstOrDefaultAsync(pb => pb.PurchaseBatchId == dto.PurchaseBatchId);

            if (batch == null)
                throw new InvalidOperationException("Purchase batch not found");

            var totalPaid = batch.Payments.Sum(p => p.AmountPaid);
            var outstanding = batch.TotalAmount - totalPaid;

            if (dto.AmountPaid > outstanding)
                throw new ArgumentException($"Payment amount ({dto.AmountPaid}) exceeds outstanding amount ({outstanding})");

            var payment = new PurchasePayment
            {
                PurchasePaymentId = Guid.NewGuid(),
                PurchaseBatchId = dto.PurchaseBatchId,
                AmountPaid = dto.AmountPaid,
                PaymentDate = dto.PaymentDate,
                DueDate = dto.DueDate,
                PaymentMode = dto.PaymentMode,
                PaymentStatus = dto.PaymentDate.HasValue ? "Paid" : "Pending",
                Remarks = dto.Remarks
            };

            _context.PurchasePayments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<IEnumerable<PurchasePayment>> GetPaymentsByBatchAsync(Guid purchaseBatchId)
        {
            return await _context.PurchasePayments
                .Where(pp => pp.PurchaseBatchId == purchaseBatchId)
                .OrderBy(pp => pp.DueDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaidAsync(Guid purchaseBatchId)
        {
            return await _context.PurchasePayments
                .Where(pp => pp.PurchaseBatchId == purchaseBatchId)
                .SumAsync(pp => pp.AmountPaid);
        }

        public async Task<IEnumerable<PurchasePayment>> GetOverduePaymentsAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.PurchasePayments
                .Where(pp => pp.PaymentStatus == "Pending" && pp.DueDate < today)
                .OrderBy(pp => pp.DueDate)
                .ToListAsync();
        }

        public async Task<bool> MarkPaymentAsPaidAsync(Guid paymentId, DateTime paymentDate)
        {
            var payment = await _context.PurchasePayments.FindAsync(paymentId);
            if (payment == null)
                return false;

            payment.PaymentDate = paymentDate;
            payment.PaymentStatus = "Paid";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
