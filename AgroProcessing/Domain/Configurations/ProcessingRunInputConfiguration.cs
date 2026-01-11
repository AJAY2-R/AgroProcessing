using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProcessingRunInputConfiguration : IEntityTypeConfiguration<ProcessingRunInput>
    {
        public void Configure(EntityTypeBuilder<ProcessingRunInput> builder)
        {
            builder.HasKey(pri => pri.ProcessingRunInputId);
            builder.Property(pri => pri.InputWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<ProcessingRun>()
                   .WithMany()
                   .HasForeignKey(pri => pri.ProcessingRunId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<PurchaseBatch>()
                   .WithMany()
                   .HasForeignKey(pri => pri.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
