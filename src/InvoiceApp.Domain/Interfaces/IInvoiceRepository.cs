using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IInvoiceRepository
{
    Task<(IEnumerable<Invoice> Items, int TotalCount)> GetInvoicesPaged(
        int page,
        int pageSize,
        string? invoiceNumber = null,
        int? clientId = null,
        int? sellerId = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        string? status = null);

    Task<Invoice?> GetInvoiceById(int id);
    Task<bool> InvoiceNumberExists(string invoiceNumber);
    Task<string> GenerateNextInvoiceNumber();
    Task AddInvoice(Invoice invoice);
    void UpdateInvoice(Invoice invoice);
}
