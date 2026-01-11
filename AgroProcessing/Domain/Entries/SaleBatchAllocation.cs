namespace AgroProcessing.Domain.Entries
{
    public class SaleBatchAllocation
    {
        public Guid SaleBatchAllocationId { get; set; }

        public Guid SaleItemId { get; set; }
        public Guid PurchaseBatchId { get; set; }

        public decimal QuantityAllocated { get; set; }
    }
}
