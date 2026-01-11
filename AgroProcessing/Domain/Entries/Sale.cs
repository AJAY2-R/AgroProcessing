namespace AgroProcessing.Domain.Entries
{
    public class Sale
    {
        public Guid SaleId { get; set; }

        public Guid BuyerId { get; set; }
        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Open";
    }
}
