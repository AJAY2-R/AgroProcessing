namespace AgroProcessing.Domain.Entries
{
    public class Farmer
    {
        public Guid FarmerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
    }

}
