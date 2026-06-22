using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using InvoiceApp.Infrastructure.Repositories;

namespace InvoiceApp.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;

    public IUserRepository    Users    { get; }
    public IClientRepository  Clients  { get; }
    public IProductRepository Products { get; }
    public IInvoiceRepository Invoices { get; }
    public IInvoiceDetailRepository InvoiceDetails { get; }
    public IInvoicePaymentRepository InvoicePayments { get; }
    public IPaymentMethodRepository PaymentMethods { get; }

    public UnitOfWork(AppDbContext ctx)
    {
        _ctx      = ctx;
        Users     = new UserRepository(ctx);
        Clients   = new ClientRepository(ctx);
        Products  = new ProductRepository(ctx);
        Invoices = new InvoiceRepository(ctx);
        InvoiceDetails = new InvoiceDetailRepository(ctx);
        InvoicePayments = new InvoicePaymentRepository(ctx);
        PaymentMethods = new PaymentMethodRepository(ctx);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _ctx.SaveChangesAsync(cancellationToken);

    public void Dispose() => _ctx.Dispose();
}
