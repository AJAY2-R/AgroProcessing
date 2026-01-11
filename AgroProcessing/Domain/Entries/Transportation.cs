namespace AgroProcessing.Domain.Entries
{
    public class Transportation
    {
        public Guid TransportationId { get; set; }

        public string RelatedType { get; set; } = null!; // Purchase, Processing, Sale
        public Guid RelatedId { get; set; }

        public Guid FromLocationId { get; set; }
        public Guid ToLocationId { get; set; }

        public decimal Cost { get; set; }
        public string PaymentStatus { get; set; } = null!;
    }
}
