namespace AgroProcessing.Domain.Entries
{
    public class SalesPayment
    {
        public Guid SalesPaymentId { get; set; }

        public Guid SaleId { get; set; }

        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DueDate { get; set; }

        public string PaymentMode { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
    }
}
