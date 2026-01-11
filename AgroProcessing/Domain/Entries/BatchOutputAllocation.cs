namespace AgroProcessing.Domain.Entries
{
    public class BatchOutputAllocation
    {
        public Guid BatchOutputAllocationId { get; set; }

        public Guid ProcessingRunId { get; set; }
        public Guid PurchaseBatchId { get; set; }

        public decimal InputWeight { get; set; }
        public decimal OutputWeight { get; set; }
        public decimal AllocatedProcessingCost { get; set; }
    }
}
