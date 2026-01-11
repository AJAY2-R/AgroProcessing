namespace AgroProcessing.Domain.Entries
{
    public class ProcessingRunInput
    {
        public Guid ProcessingRunInputId { get; set; }

        public Guid ProcessingRunId { get; set; }
        public Guid PurchaseBatchId { get; set; }

        public decimal InputWeight { get; set; }
    }
}
