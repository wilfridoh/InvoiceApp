using InvoiceApp.Application.DTOs.Invoices;

namespace InvoiceApp.Application.Interfaces;

public interface IInvoiceDetailService
{
    Task<List<InvoiceDetailResponse>> GetByInvoiceId(int invoiceId);
    Task<List<InvoiceDetailResponse>> ReplaceByInvoiceId(int invoiceId, List<CreateInvoiceDetailRequest> details);
}
