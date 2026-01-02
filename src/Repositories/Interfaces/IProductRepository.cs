using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IProductRepository
{
    Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
    Task<ProductDto> PatchProduct(ProductDto productDto);
    Task DeleteProduct(Guid productId);
}