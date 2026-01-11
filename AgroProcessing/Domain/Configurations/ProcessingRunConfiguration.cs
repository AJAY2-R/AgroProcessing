using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProcessingRunConfiguration : IEntityTypeConfiguration<ProcessingRun>
    {
        public void Configure(EntityTypeBuilder<ProcessingRun> builder)
        {
            builder.HasKey(pr => pr.ProcessingRunId);
            builder.Property(pr => pr.StartDate)
                   .IsRequired();
            builder.Property(pr => pr.EndDate);
            builder.Property(pr => pr.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Started");
            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(pr => pr.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
