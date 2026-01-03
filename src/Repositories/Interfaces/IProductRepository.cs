using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
    public Task<ProductDto> GetProductById(Guid productId);
    public Task<ProductDto> UpdatePrice(Guid productId, float newPrice);
    public Task DeleteProduct(Guid productId);
}