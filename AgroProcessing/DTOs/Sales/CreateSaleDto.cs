namespace AgroProcessing.DTOs.Sales
{
    public class CreateSaleDto
    {
        public Guid BuyerId { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }

    public class SaleItemDto
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
    }
}
