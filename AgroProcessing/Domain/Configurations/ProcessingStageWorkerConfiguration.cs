using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProcessingStageWorkerConfiguration : IEntityTypeConfiguration<ProcessingStageWorker>
    {
        public void Configure(EntityTypeBuilder<ProcessingStageWorker> builder)
        {
            builder.HasKey(psw => psw.ProcessingStageWorkerId);
            builder.Property(psw => psw.WorkedDays)
                   .IsRequired();
            builder.Property(psw => psw.CalculatedCost)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<ProcessingStage>()
                   .WithMany()
                   .HasForeignKey(psw => psw.ProcessingStageId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Worker>()
                   .WithMany()
                   .HasForeignKey(psw => psw.WorkerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
