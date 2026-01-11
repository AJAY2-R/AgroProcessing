using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class WorkTypeConfiguration : IEntityTypeConfiguration<WorkType>
    {
        public void Configure(EntityTypeBuilder<WorkType> builder)
        {
            builder.HasKey(wt => wt.WorkTypeId);
            builder.Property(wt => wt.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(wt => wt.RateType)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(wt => wt.Rate)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");
            builder.Property(wt => wt.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
