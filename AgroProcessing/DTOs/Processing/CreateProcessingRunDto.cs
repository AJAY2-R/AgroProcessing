namespace AgroProcessing.DTOs.Processing
{
    public class CreateProcessingRunDto
    {
        public Guid ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public List<ProcessingRunInputDto> Inputs { get; set; } = new();
    }

    public class ProcessingRunInputDto
    {
        public Guid PurchaseBatchId { get; set; }
        public decimal InputWeight { get; set; }
    }
}
