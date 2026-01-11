using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(w => w.WorkerId);
            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(w => w.SkillType)
                   .HasMaxLength(50);
            builder.Property(w => w.DefaultRate)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");
            builder.Property(w => w.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
