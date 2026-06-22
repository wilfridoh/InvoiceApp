using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _ctx;

    public PaymentMethodRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public Task<List<PaymentMethod>> GetAllPaymentMethodsActive() =>
        _ctx.PaymentMethods.Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync();

    public Task<PaymentMethod?> GetPaymentMethodById(int id) =>
        _ctx.PaymentMethods.FirstOrDefaultAsync(x => x.Id == id);
}
