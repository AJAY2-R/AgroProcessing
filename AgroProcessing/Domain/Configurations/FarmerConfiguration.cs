using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class FarmerConfiguration : IEntityTypeConfiguration<Farmer>
    {
        public void Configure(EntityTypeBuilder<Farmer> builder)
        {
            builder.HasKey(f => f.FarmerId);
            builder.Property(f => f.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(f => f.Phone)
                   .HasMaxLength(20);
        }
    }
}
