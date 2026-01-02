using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IProductRepository
{
    Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
    Task<ProductDto> UpdatePrice(Guid productId, float newPrice);
    Task DeleteProduct(Guid productId);
}