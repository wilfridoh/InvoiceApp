using InvoiceApp.Application.DTOs.Invoices;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class InvoiceDetailService : IInvoiceDetailService
{
    private readonly IUnitOfWork _uow;

    public InvoiceDetailService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<InvoiceDetailResponse>> GetByInvoiceId(int invoiceId)
    {
        var details = await _uow.InvoiceDetails.GetByInvoiceId(invoiceId);
        return details.Select(Map).ToList();
    }

    public async Task<List<InvoiceDetailResponse>> ReplaceByInvoiceId(int invoiceId, List<CreateInvoiceDetailRequest> details)
    {
        var invoice = await _uow.Invoices.GetInvoiceById(invoiceId)
            ?? throw new KeyNotFoundException($"Factura {invoiceId} no encontrada.");

        _ = invoice;

        var entities = new List<InvoiceDetail>();

        foreach (var d in details)
        {
            if (d.Quantity <= 0 || d.UnitPrice <= 0)
                throw new InvalidOperationException("Cantidad y precio unitario deben ser mayores a 0.");

            entities.Add(new InvoiceDetail
            {
                InvoiceId = invoiceId,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                TotalPrice = d.Quantity * d.UnitPrice
            });
        }

        await _uow.InvoiceDetails.RemoveByInvoiceId(invoiceId);
        await _uow.InvoiceDetails.AddRange(entities);
        await _uow.SaveChangesAsync();

        return entities.Select(Map).ToList();
    }

    private static InvoiceDetailResponse Map(InvoiceDetail d) => new()
    {
        Id = d.Id,
        InvoiceId = d.InvoiceId,
        ProductId = d.ProductId,
        Quantity = d.Quantity,
        UnitPrice = d.UnitPrice,
        TotalPrice = d.TotalPrice
    };
}
