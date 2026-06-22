using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IProductRepository
{
    Task<(IEnumerable<Product> Items, int TotalCount)> GetProductsPaged(int page, int pageSize, string? search = null);
    Task<Product?> GetProductById(int id);
    Task<bool> CodeExists(string code, int? excludeId = null);
    Task AddProduct(Product product);
    void UpdateProduct(Product product);
}
