using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.DTOs.Invoices;

namespace InvoiceApp.Application.Interfaces;

public interface IInvoiceService
{
    Task<PagedResult<InvoiceResponse>> GetInvoices(InvoiceListFilters filters);
    Task<InvoiceResponse> GetInvoiceById(int id);
    Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request);
    Task Cancel(int id);
}
