namespace InvoiceApp.Application.DTOs.Invoices;

public class InvoiceResponse
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public int SellerId { get; set; }
    public DateTime Date { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<InvoiceDetailResponse> Details { get; set; } = [];
    public List<InvoicePaymentResponse> Payments { get; set; } = [];
}

public class InvoiceDetailResponse
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class InvoicePaymentResponse
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
}
