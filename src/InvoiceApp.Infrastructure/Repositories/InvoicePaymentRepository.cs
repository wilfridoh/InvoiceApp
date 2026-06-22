using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class InvoicePaymentRepository : IInvoicePaymentRepository
{
    private readonly AppDbContext _ctx;

    public InvoicePaymentRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public Task<List<InvoicePayment>> GetByInvoiceId(int invoiceId) =>
        _ctx.InvoicePayments.Where(x => x.InvoiceId == invoiceId).ToListAsync();

    public async Task AddRange(IEnumerable<InvoicePayment> payments) =>
        await _ctx.InvoicePayments.AddRangeAsync(payments);

    public async Task RemoveByInvoiceId(int invoiceId)
    {
        var current = await _ctx.InvoicePayments.Where(x => x.InvoiceId == invoiceId).ToListAsync();
        if (current.Count > 0)
            _ctx.InvoicePayments.RemoveRange(current);
    }
}
