using InvoiceApp.Application.DTOs.PaymentMethods;

namespace InvoiceApp.Application.Interfaces;

public interface IPaymentMethodService
{
    Task<List<PaymentMethodResponse>> GetAllActive();
}
