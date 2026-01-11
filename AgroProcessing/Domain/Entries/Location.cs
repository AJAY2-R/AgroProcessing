namespace AgroProcessing.Domain.Entries
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; } = null!;
        public string LocationType { get; set; } = null!; // Raw, Processing, Drying, Finished
    }
}
