namespace InvoiceApp.Application.DTOs.PaymentMethods;

public class PaymentMethodResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
