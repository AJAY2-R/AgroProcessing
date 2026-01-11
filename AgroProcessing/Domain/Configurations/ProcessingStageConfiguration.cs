using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProcessingStageConfiguration : IEntityTypeConfiguration<ProcessingStage>
    {
        public void Configure(EntityTypeBuilder<ProcessingStage> builder)
        {
            builder.HasKey(ps => ps.ProcessingStageId);
            builder.Property(ps => ps.StartDate)
                   .IsRequired();
            builder.Property(ps => ps.EndDate);
            builder.HasOne<ProcessingRun>()
                   .WithMany()
                   .HasForeignKey(ps => ps.ProcessingRunId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<WorkType>()
                   .WithMany()
                   .HasForeignKey(ps => ps.WorkTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
