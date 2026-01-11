namespace AgroProcessing.DTOs.Processing
{
    public class CompleteProcessingRunDto
    {
        public Guid ProcessingRunId { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalOutputWeight { get; set; }
        public List<AdditionalCostDto> AdditionalCosts { get; set; } = new();
    }

    public class AdditionalCostDto
    {
        public string CostType { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
