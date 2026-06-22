namespace InvoiceApp.Application.DTOs.Clients;

public class CreateClientRequest
{
    public string Name { get; set; } = string.Empty;
    public string DocumentType { get; set; } = "RUC";
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
