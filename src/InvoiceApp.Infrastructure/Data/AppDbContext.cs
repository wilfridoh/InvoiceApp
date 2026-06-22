using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User>    Users    => Set<User>();
    public DbSet<Client>  Clients  => Set<Client>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
    public DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new ClientConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new InvoiceConfiguration());
        builder.ApplyConfiguration(new InvoiceDetailConfiguration());
        builder.ApplyConfiguration(new InvoicePaymentConfiguration());
        builder.ApplyConfiguration(new PaymentMethodConfiguration());
    }
}
