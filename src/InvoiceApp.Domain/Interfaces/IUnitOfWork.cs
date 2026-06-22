namespace InvoiceApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository    Users    { get; }
    IClientRepository  Clients  { get; }
    IProductRepository Products { get; }
    IInvoiceRepository Invoices { get; }
    IInvoiceDetailRepository InvoiceDetails { get; }
    IInvoicePaymentRepository InvoicePayments { get; }
    IPaymentMethodRepository PaymentMethods { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
