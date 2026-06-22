namespace InvoiceApp.Domain.Entities;

public class InvoicePayment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
}
