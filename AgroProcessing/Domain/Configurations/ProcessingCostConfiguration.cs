using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProcessingCostConfiguration : IEntityTypeConfiguration<ProcessingCost>
    {
        public void Configure(EntityTypeBuilder<ProcessingCost> builder)
        {
            builder.HasKey(pc => pc.ProcessingCostId);
            builder.Property(pc => pc.CostType)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(pc => pc.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<ProcessingRun>()
                   .WithMany()
                   .HasForeignKey(pc => pc.ProcessingRunId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
