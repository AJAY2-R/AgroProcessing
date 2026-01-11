using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.HasKey(b => b.BuyerId);
            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(b => b.Phone)
                   .HasMaxLength(20);
            builder.Property(b => b.CreditLimit)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            
            builder.HasIndex(b => b.Name);
        }
    }
}
