using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class BatchOutputAllocationConfiguration : IEntityTypeConfiguration<BatchOutputAllocation>
    {
        public void Configure(EntityTypeBuilder<BatchOutputAllocation> builder)
        {
            builder.HasKey(boa => boa.BatchOutputAllocationId);
            builder.Property(boa => boa.InputWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(boa => boa.OutputWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(boa => boa.AllocatedProcessingCost)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<ProcessingRun>()
                   .WithMany()
                   .HasForeignKey(boa => boa.ProcessingRunId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<PurchaseBatch>()
                   .WithMany()
                   .HasForeignKey(boa => boa.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
