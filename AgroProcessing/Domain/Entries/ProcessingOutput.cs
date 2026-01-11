namespace AgroProcessing.Domain.Entries
{
    public class ProcessingOutput
    {
        public Guid ProcessingOutputId { get; set; }

        public Guid ProcessingRunId { get; set; }

        public decimal TotalInputWeight { get; set; }
        public decimal TotalOutputWeight { get; set; }
        public decimal TotalLossWeight { get; set; }
        public decimal YieldPercent { get; set; }
    }
}
