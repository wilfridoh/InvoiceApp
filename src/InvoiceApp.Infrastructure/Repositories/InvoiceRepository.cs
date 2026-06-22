using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _ctx;

    public InvoiceRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<(IEnumerable<Invoice> Items, int TotalCount)> GetInvoicesPaged(
        int page,
        int pageSize,
        string? invoiceNumber = null,
        int? clientId = null,
        int? sellerId = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        string? status = null)
    {
        var query = _ctx.Invoices.AsQueryable();

        if (!string.IsNullOrWhiteSpace(invoiceNumber))
            query = query.Where(x => x.InvoiceNumber.Contains(invoiceNumber));

        if (clientId.HasValue)
            query = query.Where(x => x.ClientId == clientId.Value);

        if (sellerId.HasValue)
            query = query.Where(x => x.SellerId == sellerId.Value);

        if (dateFrom.HasValue)
            query = query.Where(x => x.Date >= dateFrom.Value.Date);

        if (dateTo.HasValue)
            query = query.Where(x => x.Date <= dateTo.Value.Date);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Status == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public Task<Invoice?> GetInvoiceById(int id) =>
        _ctx.Invoices.FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> InvoiceNumberExists(string invoiceNumber) =>
        _ctx.Invoices.AnyAsync(x => x.InvoiceNumber == invoiceNumber);

    public async Task<string> GenerateNextInvoiceNumber()
    {
        var next = (await _ctx.Invoices.MaxAsync(x => (int?)x.Id) ?? 0) + 1;
        return $"FAC-{next:000000}";
    }

    public async Task AddInvoice(Invoice invoice) => await _ctx.Invoices.AddAsync(invoice);

    public void UpdateInvoice(Invoice invoice) => _ctx.Invoices.Update(invoice);
}
