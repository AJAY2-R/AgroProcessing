namespace AgroProcessing.Domain.Entries
{
    public class Buyer
    {
        public Guid BuyerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
