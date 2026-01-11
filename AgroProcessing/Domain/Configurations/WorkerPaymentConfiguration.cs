using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class WorkerPaymentConfiguration : IEntityTypeConfiguration<WorkerPayment>
    {
        public void Configure(EntityTypeBuilder<WorkerPayment> builder)
        {
            builder.HasKey(wp => wp.WorkerPaymentId);
            builder.Property(wp => wp.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(wp => wp.PaidStatus)
                   .IsRequired();
            builder.Property(wp => wp.PaymentDate);
            builder.HasOne<Worker>()
                   .WithMany()
                   .HasForeignKey(wp => wp.WorkerId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<ProcessingStage>()
                   .WithMany()
                   .HasForeignKey(wp => wp.ProcessingStageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
