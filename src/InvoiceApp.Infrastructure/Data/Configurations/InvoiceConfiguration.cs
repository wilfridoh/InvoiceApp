using InvoiceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceApp.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.InvoiceNumber).IsUnique();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Subtotal)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Tax)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Total)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Issued");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
