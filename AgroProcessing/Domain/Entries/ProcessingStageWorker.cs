namespace AgroProcessing.Domain.Entries
{
    public class ProcessingStageWorker
    {
        public Guid ProcessingStageWorkerId { get; set; }

        public Guid ProcessingStageId { get; set; }
        public Guid WorkerId { get; set; }

        public int WorkedDays { get; set; }
        public decimal CalculatedCost { get; set; }
    }
}
