namespace AgroProcessing.DTOs.Purchase
{
    public class CreatePurchaseBatchDto
    {
        public Guid FarmerId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal RawWeight { get; set; }
        public decimal RatePerKg { get; set; }
        public string PaymentSettlementType { get; set; } = null!; // BeforeProcessing, AfterYield
        public decimal? InitialPayment { get; set; }
        public string? PaymentMode { get; set; }
    }
}
