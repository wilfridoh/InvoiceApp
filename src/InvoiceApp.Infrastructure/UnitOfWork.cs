using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using InvoiceApp.Infrastructure.Repositories;

namespace InvoiceApp.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;

    public IUserRepository   Users   { get; }
    public IClientRepository Clients { get; }

    public UnitOfWork(AppDbContext ctx)
    {
        _ctx    = ctx;
        Users   = new UserRepository(ctx);
        Clients = new ClientRepository(ctx);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _ctx.SaveChangesAsync(cancellationToken);

    public void Dispose() => _ctx.Dispose();
}
