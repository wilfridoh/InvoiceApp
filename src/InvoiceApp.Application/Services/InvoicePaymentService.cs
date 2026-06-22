using InvoiceApp.Application.DTOs.Invoices;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class InvoicePaymentService : IInvoicePaymentService
{
    private readonly IUnitOfWork _uow;

    public InvoicePaymentService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<InvoicePaymentResponse>> GetByInvoiceId(int invoiceId)
    {
        var payments = await _uow.InvoicePayments.GetByInvoiceId(invoiceId);
        return payments.Select(Map).ToList();
    }

    public async Task<List<InvoicePaymentResponse>> ReplaceByInvoiceId(int invoiceId, List<CreateInvoicePaymentRequest> payments)
    {
        var invoice = await _uow.Invoices.GetInvoiceById(invoiceId)
            ?? throw new KeyNotFoundException($"Factura {invoiceId} no encontrada.");

        _ = invoice;

        var entities = new List<InvoicePayment>();

        foreach (var p in payments)
        {
            if (p.Amount <= 0)
                throw new InvalidOperationException("El monto debe ser mayor a 0.");

            var method = await _uow.PaymentMethods.GetPaymentMethodById(p.PaymentMethodId);
            if (method is null || !method.IsActive)
                throw new InvalidOperationException($"Forma de pago {p.PaymentMethodId} no válida.");

            entities.Add(new InvoicePayment
            {
                InvoiceId = invoiceId,
                PaymentMethodId = p.PaymentMethodId,
                Amount = p.Amount
            });
        }

        await _uow.InvoicePayments.RemoveByInvoiceId(invoiceId);
        await _uow.InvoicePayments.AddRange(entities);
        await _uow.SaveChangesAsync();

        return entities.Select(Map).ToList();
    }

    private static InvoicePaymentResponse Map(InvoicePayment p) => new()
    {
        Id = p.Id,
        InvoiceId = p.InvoiceId,
        PaymentMethodId = p.PaymentMethodId,
        Amount = p.Amount
    };
}
