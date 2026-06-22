using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IInvoiceDetailRepository
{
    Task<List<InvoiceDetail>> GetByInvoiceId(int invoiceId);
    Task AddRange(IEnumerable<InvoiceDetail> details);
    Task RemoveByInvoiceId(int invoiceId);
}
