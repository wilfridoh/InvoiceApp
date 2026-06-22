using InvoiceApp.Application.DTOs.Invoices;

namespace InvoiceApp.Application.Interfaces;

public interface IInvoicePaymentService
{
    Task<List<InvoicePaymentResponse>> GetByInvoiceId(int invoiceId);
    Task<List<InvoicePaymentResponse>> ReplaceByInvoiceId(int invoiceId, List<CreateInvoicePaymentRequest> payments);
}
