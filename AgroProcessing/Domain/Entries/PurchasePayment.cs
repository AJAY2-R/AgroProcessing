namespace AgroProcessing.Domain.Entries
{
    public class PurchasePayment
    {
        public Guid PurchasePaymentId { get; set; }

        public Guid PurchaseBatchId { get; set; }

        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DueDate { get; set; }

        public string PaymentMode { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string? Remarks { get; set; }
    }
}
