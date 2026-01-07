using EfCoreApiExample.src.DTOs;

namespace EfCoreApiExample.src.Services.Interfaces;

public interface IProductService
{
    public Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    public Task<ProductDto> GetProductByIdAsync(Guid productId);
    public Task<ProductDto> GetProductByNameAsync(string productName);
    public Task<ProductDto> UpdateProductAsync(ProductDto productDto);
    public Task DeleteProductAsync(Guid productId);
}