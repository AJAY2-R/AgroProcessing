namespace AgroProcessing.Domain.Entries
{
    public class SaleItem
    {
        public Guid SaleItemId { get; set; }

        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
    }
}
