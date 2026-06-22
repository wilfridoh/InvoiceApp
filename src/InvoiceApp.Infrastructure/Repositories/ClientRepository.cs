using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _ctx;

    public ClientRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<Client> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null)
    {
        var query = _ctx.Clients
            .Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(term) ||
                c.DocumentNumber.Contains(term) ||
                c.Email.ToLower().Contains(term));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public Task<Client?> GetByIdAsync(int id) =>
        _ctx.Clients.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

    public Task<bool> DocumentExistsAsync(string documentNumber, int? excludeId = null)
    {
        var query = _ctx.Clients
            .Where(c => c.DocumentNumber == documentNumber && c.IsActive);

        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);

        return query.AnyAsync();
    }

    public async Task AddAsync(Client client) => await _ctx.Clients.AddAsync(client);

    public void Update(Client client) => _ctx.Clients.Update(client);
}
