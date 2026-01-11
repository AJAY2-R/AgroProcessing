namespace AgroProcessing.Domain.Entries
{
    public class ProcessingRun
    {
        public Guid ProcessingRunId { get; set; }

        public Guid ProductId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Started";
    }
}
