using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    public Task<ProductDto> GetProductByIdAsync(Guid productId);
    public Task<ProductDto> UpdatePriceAsync(Guid productId, float newPrice);
    public Task DeleteProductAsync(Guid productId);
}