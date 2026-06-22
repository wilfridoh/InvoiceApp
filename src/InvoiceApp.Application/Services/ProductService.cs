using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.DTOs.Products;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;

    public ProductService(IUnitOfWork uow) => _uow = uow;

    public async Task<PagedResult<ProductResponse>> GetProductsPaged(int page, int pageSize, string? search = null)
    {
        var (items, total) = await _uow.Products.GetProductsPaged(page, pageSize, search);
        return new PagedResult<ProductResponse>
        {
            Items = items.Select(ToResponse),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ProductResponse> GetProductById(int id)
    {
        var product = await _uow.Products.GetProductById(id)
            ?? throw new KeyNotFoundException($"Producto {id} no encontrado.");
        return ToResponse(product);
    }

    public async Task<ProductResponse> CreateProduct(CreateProductRequest request)
    {
        if (await _uow.Products.CodeExists(request.Code))
            throw new InvalidOperationException($"Ya existe un producto con el código '{request.Code}'.");

        var product = new Product
        {
            Code = request.Code.Trim().ToUpper(),
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price,
            Stock = request.Stock,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Products.AddProduct(product);
        await _uow.SaveChangesAsync();
        return ToResponse(product);
    }

    public async Task<ProductResponse> UpdateProduct(int id, UpdateProductRequest request)
    {
        var product = await _uow.Products.GetProductById(id)
            ?? throw new KeyNotFoundException($"Producto {id} no encontrado.");

        if (await _uow.Products.CodeExists(request.Code, excludeId: id))
            throw new InvalidOperationException($"Ya existe otro producto con el código '{request.Code}'.");

        product.Code = request.Code.Trim().ToUpper();
        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Price = request.Price;
        product.Stock = request.Stock;

        _uow.Products.UpdateProduct(product);
        await _uow.SaveChangesAsync();
        return ToResponse(product);
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _uow.Products.GetProductById(id)
            ?? throw new KeyNotFoundException($"Producto {id} no encontrado.");

        product.IsActive = false;
        _uow.Products.UpdateProduct(product);
        await _uow.SaveChangesAsync();
    }

    private static ProductResponse ToResponse(Product p) => new()
    {
        Id = p.Id,
        Code = p.Code,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt
    };
}
