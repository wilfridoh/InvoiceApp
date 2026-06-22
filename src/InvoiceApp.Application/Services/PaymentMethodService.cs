using InvoiceApp.Application.DTOs.PaymentMethods;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IUnitOfWork _uow;

    public PaymentMethodService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<PaymentMethodResponse>> GetAllActive()
    {
        var methods = await _uow.PaymentMethods.GetAllPaymentMethodsActive();
        return methods.Select(m => new PaymentMethodResponse
        {
            Id = m.Id,
            Name = m.Name,
            IsActive = m.IsActive
        }).ToList();
    }
}
