namespace AgroProcessing.Domain.Entries
{
    public class Worker
    {
        public Guid WorkerId { get; set; }
        public string Name { get; set; } = null!;
        public string? SkillType { get; set; }
        public decimal DefaultRate { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
