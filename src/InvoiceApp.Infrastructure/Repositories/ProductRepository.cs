using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _ctx;

    public ProductRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetProductsPaged(
        int page, int pageSize, string? search = null)
    {
        var query = _ctx.Products.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Code.ToLower().Contains(term) ||
                p.Description.ToLower().Contains(term));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public Task<Product?> GetProductById(int id) =>
        _ctx.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    public Task<bool> CodeExists(string code, int? excludeId = null)
    {
        var query = _ctx.Products
            .Where(p => p.Code == code.ToUpper() && p.IsActive);

        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);

        return query.AnyAsync();
    }

    public async Task AddProduct(Product product) => await _ctx.Products.AddAsync(product);

    public void UpdateProduct(Product product) => _ctx.Products.Update(product);
}
