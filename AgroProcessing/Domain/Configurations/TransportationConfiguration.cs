using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class TransportationConfiguration : IEntityTypeConfiguration<Transportation>
    {
        public void Configure(EntityTypeBuilder<Transportation> builder)
        {
            builder.HasKey(t => t.TransportationId);
            builder.Property(t => t.RelatedType)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(t => t.Cost)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(t => t.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasOne<Location>()
                   .WithMany()
                   .HasForeignKey(t => t.FromLocationId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Location>()
                   .WithMany()
                   .HasForeignKey(t => t.ToLocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
