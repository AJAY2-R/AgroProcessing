namespace AgroProcessing.DTOs.Purchase
{
    public class RecordPaymentDto
    {
        public Guid PurchaseBatchId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public string PaymentMode { get; set; } = null!;
        public string? Remarks { get; set; }
    }
}
