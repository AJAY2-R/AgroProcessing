using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.LocationId);
            builder.Property(l => l.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(l => l.LocationType)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}
