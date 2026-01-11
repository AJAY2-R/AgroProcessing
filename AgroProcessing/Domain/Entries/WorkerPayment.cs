namespace AgroProcessing.Domain.Entries
{
    public class WorkerPayment
    {
        public Guid WorkerPaymentId { get; set; }

        public Guid WorkerId { get; set; }
        public Guid ProcessingStageId { get; set; }

        public decimal Amount { get; set; }
        public bool PaidStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
