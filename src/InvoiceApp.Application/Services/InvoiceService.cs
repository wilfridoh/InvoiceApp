using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.DTOs.Invoices;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class InvoiceService : IInvoiceService
{
    private const decimal TaxRate = 0.12m;
    private readonly IUnitOfWork _uow;

    public InvoiceService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<InvoiceResponse>> GetInvoices(InvoiceListFilters filters)
    {
        var (items, total) = await _uow.Invoices.GetInvoicesPaged(
            filters.Page,
            filters.PageSize,
            filters.InvoiceNumber,
            filters.ClientId,
            filters.SellerId,
            filters.DateFrom,
            filters.DateTo,
            filters.Status);

        return new PagedResult<InvoiceResponse>
        {
            Items = items.Select(MapBase),
            TotalCount = total,
            Page = filters.Page,
            PageSize = filters.PageSize
        };
    }

    public async Task<InvoiceResponse> GetInvoiceById(int id)
    {
        var invoice = await _uow.Invoices.GetInvoiceById(id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        var details = await _uow.InvoiceDetails.GetByInvoiceId(id);
        var payments = await _uow.InvoicePayments.GetByInvoiceId(id);

        return MapFull(invoice, details, payments);
    }

    public async Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request)
    {
        if (request.Details.Count == 0)
            throw new InvalidOperationException("La factura debe incluir al menos un detalle.");

        if (request.Payments.Count == 0)
            throw new InvalidOperationException("La factura debe incluir al menos un pago.");

        var client = await _uow.Clients.GetByIdAsync(request.ClientId)
            ?? throw new InvalidOperationException("Cliente no válido.");

        var seller = await _uow.Users.GetByIdAsync(request.SellerId)
            ?? throw new InvalidOperationException("Vendedor no válido.");

        _ = client;
        _ = seller;

        decimal subtotal = 0m;
        var detailEntities = new List<InvoiceDetail>();

        foreach (var d in request.Details)
        {
            var product = await _uow.Products.GetProductById(d.ProductId)
                ?? throw new InvalidOperationException($"Producto {d.ProductId} no válido.");

            if (d.Quantity <= 0)
                throw new InvalidOperationException("Cantidad debe ser mayor a 0.");

            if (d.UnitPrice <= 0)
                throw new InvalidOperationException("Precio unitario debe ser mayor a 0.");

            if (product.Stock < d.Quantity)
                throw new InvalidOperationException($"Stock insuficiente para producto {product.Code}.");

            var lineTotal = d.Quantity * d.UnitPrice;
            subtotal += lineTotal;

            detailEntities.Add(new InvoiceDetail
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                TotalPrice = lineTotal
            });

            product.Stock -= d.Quantity;
            _uow.Products.UpdateProduct(product);
        }

        var tax = Math.Round(subtotal * TaxRate, 2, MidpointRounding.AwayFromZero);
        var total = subtotal + tax;

        var paymentsTotal = request.Payments.Sum(p => p.Amount);
        if (Math.Abs(paymentsTotal - total) > 0.01m)
            throw new InvalidOperationException("La suma de pagos debe ser igual al total de la factura.");

        foreach (var p in request.Payments)
        {
            var method = await _uow.PaymentMethods.GetPaymentMethodById(p.PaymentMethodId);
            if (method is null || !method.IsActive)
                throw new InvalidOperationException($"Forma de pago {p.PaymentMethodId} no válida.");

            if (p.Amount <= 0)
                throw new InvalidOperationException("Monto de pago debe ser mayor a 0.");
        }

        var invoice = new Invoice
        {
            InvoiceNumber = await _uow.Invoices.GenerateNextInvoiceNumber(),
            ClientId = request.ClientId,
            SellerId = request.SellerId,
            Date = request.Date == default ? DateTime.UtcNow : request.Date,
            Subtotal = subtotal,
            Tax = tax,
            Total = total,
            Status = "Issued",
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Invoices.AddInvoice(invoice);
        await _uow.SaveChangesAsync();

        detailEntities.ForEach(d => d.InvoiceId = invoice.Id);

        var paymentEntities = request.Payments.Select(p => new InvoicePayment
        {
            InvoiceId = invoice.Id,
            PaymentMethodId = p.PaymentMethodId,
            Amount = p.Amount
        }).ToList();

        await _uow.InvoiceDetails.AddRange(detailEntities);
        await _uow.InvoicePayments.AddRange(paymentEntities);
        await _uow.SaveChangesAsync();

        return MapFull(invoice, detailEntities, paymentEntities);
    }

    public async Task Cancel(int id)
    {
        var invoice = await _uow.Invoices.GetInvoiceById(id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        if (!string.Equals(invoice.Status, "Issued", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo facturas en estado Issued pueden cancelarse.");

        invoice.Status = "Cancelled";
        _uow.Invoices.UpdateInvoice(invoice);
        await _uow.SaveChangesAsync();
    }

    private static InvoiceResponse MapBase(Invoice i) => new()
    {
        Id = i.Id,
        InvoiceNumber = i.InvoiceNumber,
        ClientId = i.ClientId,
        SellerId = i.SellerId,
        Date = i.Date,
        Subtotal = i.Subtotal,
        Tax = i.Tax,
        Total = i.Total,
        Status = i.Status,
        CreatedAt = i.CreatedAt
    };

    private static InvoiceResponse MapFull(
        Invoice i,
        IEnumerable<InvoiceDetail> details,
        IEnumerable<InvoicePayment> payments)
    {
        var response = MapBase(i);
        response.Details = details.Select(d => new InvoiceDetailResponse
        {
            Id = d.Id,
            InvoiceId = d.InvoiceId,
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            UnitPrice = d.UnitPrice,
            TotalPrice = d.TotalPrice
        }).ToList();

        response.Payments = payments.Select(p => new InvoicePaymentResponse
        {
            Id = p.Id,
            InvoiceId = p.InvoiceId,
            PaymentMethodId = p.PaymentMethodId,
            Amount = p.Amount
        }).ToList();

        return response;
    }
}
