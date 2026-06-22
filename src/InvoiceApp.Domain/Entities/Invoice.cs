namespace InvoiceApp.Domain.Entities;

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public int SellerId { get; set; }
    public DateTime Date { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Issued";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
