namespace InvoiceApp.Application.DTOs.Invoices;

public class InvoiceListFilters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? InvoiceNumber { get; set; }
    public int? ClientId { get; set; }
    public int? SellerId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Status { get; set; }
}
