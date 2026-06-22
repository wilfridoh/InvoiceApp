using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class InvoiceDetailRepository : IInvoiceDetailRepository
{
    private readonly AppDbContext _ctx;

    public InvoiceDetailRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public Task<List<InvoiceDetail>> GetByInvoiceId(int invoiceId) =>
        _ctx.InvoiceDetails.Where(x => x.InvoiceId == invoiceId).ToListAsync();

    public async Task AddRange(IEnumerable<InvoiceDetail> details) =>
        await _ctx.InvoiceDetails.AddRangeAsync(details);

    public async Task RemoveByInvoiceId(int invoiceId)
    {
        var current = await _ctx.InvoiceDetails.Where(x => x.InvoiceId == invoiceId).ToListAsync();
        if (current.Count > 0)
            _ctx.InvoiceDetails.RemoveRange(current);
    }
}
