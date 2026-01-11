namespace AgroProcessing.Domain.Entries
{
    public class PurchaseBatch
    {
        public Guid PurchaseBatchId { get; set; }

        public Guid FarmerId { get; set; }
        public Guid ProductId { get; set; }

        public DateTime PurchaseDate { get; set; }
        public decimal RawWeight { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal TotalAmount { get; set; }

        public string PaymentSettlementType { get; set; } = null!; // BeforeProcessing, AfterYield
        public string Status { get; set; } = "Stored";

        public ICollection<PurchasePayment> Payments { get; set; } = new List<PurchasePayment>();
    }
}
