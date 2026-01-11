using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class WorkerPaymentService : IWorkerPaymentService
    {
        private readonly AppDbContext _context;

        public WorkerPaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkerPayment> CreateWorkerPaymentAsync(Guid workerId, Guid processingStageId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            var worker = await _context.Workers.FindAsync(workerId);
            if (worker == null)
                throw new InvalidOperationException("Worker not found");

            var stage = await _context.ProcessingStages.FindAsync(processingStageId);
            if (stage == null)
                throw new InvalidOperationException("Processing stage not found");

            var existingPayment = await _context.WorkerPayments
                .FirstOrDefaultAsync(wp => wp.WorkerId == workerId && wp.ProcessingStageId == processingStageId);

            if (existingPayment != null)
                throw new InvalidOperationException("Payment already exists for this worker and stage");

            var payment = new WorkerPayment
            {
                WorkerPaymentId = Guid.NewGuid(),
                WorkerId = workerId,
                ProcessingStageId = processingStageId,
                Amount = amount,
                PaidStatus = false
            };

            _context.WorkerPayments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<IEnumerable<WorkerPayment>> GetUnpaidWorkerPaymentsAsync()
        {
            return await _context.WorkerPayments
                .AsNoTracking()
                .Where(wp => !wp.PaidStatus)
                .OrderByDescending(wp => wp.Amount)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkerPayment>> GetWorkerPaymentsByWorkerAsync(Guid workerId)
        {
            return await _context.WorkerPayments
                .AsNoTracking()
                .Where(wp => wp.WorkerId == workerId)
                .OrderByDescending(wp => wp.PaymentDate)
                .ToListAsync();
        }

        public async Task<bool> MarkWorkerPaymentAsPaidAsync(Guid workerPaymentId, DateTime paymentDate)
        {
            var payment = await _context.WorkerPayments.FindAsync(workerPaymentId);
            if (payment == null)
                return false;

            payment.PaidStatus = true;
            payment.PaymentDate = paymentDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalUnpaidForWorkerAsync(Guid workerId)
        {
            return await _context.WorkerPayments
                .AsNoTracking()
                .Where(wp => wp.WorkerId == workerId && !wp.PaidStatus)
                .SumAsync(wp => wp.Amount);
        }

        public async Task<IEnumerable<WorkerPayment>> GetPaymentsByProcessingStageAsync(Guid processingStageId)
        {
            return await _context.WorkerPayments
                .AsNoTracking()
                .Where(wp => wp.ProcessingStageId == processingStageId)
                .ToListAsync();
        }
    }
}
