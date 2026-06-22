using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IInvoicePaymentRepository
{
    Task<List<InvoicePayment>> GetByInvoiceId(int invoiceId);
    Task AddRange(IEnumerable<InvoicePayment> payments);
    Task RemoveByInvoiceId(int invoiceId);
}
