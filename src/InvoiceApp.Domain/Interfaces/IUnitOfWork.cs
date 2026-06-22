namespace InvoiceApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository   Users   { get; }
    IClientRepository Clients { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
