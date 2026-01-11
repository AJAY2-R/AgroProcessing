namespace AgroProcessing.Domain.Entries
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;

        public decimal ExpectedYieldPercent { get; set; }
        public int DryingDaysMin { get; set; }
        public int DryingDaysMax { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
