namespace AgroProcessing.Domain.Entries
{
    public class FinishedInventory
    {
        public Guid FinishedInventoryId { get; set; }

        public Guid PurchaseBatchId { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }

        public decimal Quantity { get; set; }
    }
}
