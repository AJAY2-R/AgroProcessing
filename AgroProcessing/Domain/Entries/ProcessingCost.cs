namespace AgroProcessing.Domain.Entries
{
    public class ProcessingCost
    {
        public Guid ProcessingCostId { get; set; }

        public Guid ProcessingRunId { get; set; }

        public string CostType { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
