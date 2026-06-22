namespace InvoiceApp.Domain.Entities;

public class Client
{
    public int    Id             { get; set; }
    public string Name           { get; set; } = string.Empty;
    public string DocumentType   { get; set; } = "RUC";   // "RUC" | "CI" | "Pasaporte"
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email          { get; set; } = string.Empty;
    public string Phone          { get; set; } = string.Empty;
    public string Address        { get; set; } = string.Empty;
    public bool   IsActive       { get; set; } = true;
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;
}
