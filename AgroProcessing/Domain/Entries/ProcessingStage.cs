namespace AgroProcessing.Domain.Entries
{
    public class ProcessingStage
    {
        public Guid ProcessingStageId { get; set; }

        public Guid ProcessingRunId { get; set; }
        public Guid WorkTypeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
