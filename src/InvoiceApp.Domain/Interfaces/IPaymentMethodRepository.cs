using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethod>> GetAllPaymentMethodsActive();
    Task<PaymentMethod?> GetPaymentMethodById(int id);
}
