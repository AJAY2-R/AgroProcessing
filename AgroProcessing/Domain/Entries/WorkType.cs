namespace AgroProcessing.Domain.Entries
{
    public class WorkType
    {
        public Guid WorkTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string RateType { get; set; } = null!; // PerKg, Per10Kg, PerDay
        public decimal Rate { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
