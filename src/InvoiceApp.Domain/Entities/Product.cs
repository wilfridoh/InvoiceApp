namespace InvoiceApp.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;  // código único
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
