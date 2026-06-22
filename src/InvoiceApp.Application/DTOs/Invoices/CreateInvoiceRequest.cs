namespace InvoiceApp.Application.DTOs.Invoices;

public class CreateInvoiceRequest
{
    public int ClientId { get; set; }
    public int SellerId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateInvoiceDetailRequest> Details { get; set; } = [];
    public List<CreateInvoicePaymentRequest> Payments { get; set; } = [];
}

public class CreateInvoiceDetailRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateInvoicePaymentRequest
{
    public int PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
}
