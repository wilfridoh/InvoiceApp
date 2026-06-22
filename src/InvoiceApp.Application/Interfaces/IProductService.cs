using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.DTOs.Products;

namespace InvoiceApp.Application.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductResponse>> GetProductsPaged(int page, int pageSize, string? search = null);
    Task<ProductResponse> GetProductById(int id);
    Task<ProductResponse> CreateProduct(CreateProductRequest request);
    Task<ProductResponse> UpdateProduct(int id, UpdateProductRequest request);
    Task DeleteProduct(int id);
}
