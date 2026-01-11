using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProcessingOutputConfiguration : IEntityTypeConfiguration<ProcessingOutput>
    {
        public void Configure(EntityTypeBuilder<ProcessingOutput> builder)
        {
            builder.HasKey(po => po.ProcessingOutputId);
            builder.Property(po => po.TotalInputWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(po => po.TotalOutputWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(po => po.TotalLossWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(po => po.YieldPercent)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");
            builder.HasOne<ProcessingRun>()
                   .WithMany()
                   .HasForeignKey(po => po.ProcessingRunId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
