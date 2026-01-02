using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<ProductDto> CreateProduct(CreateProductDto product);
    public Task<ProductDto> PatchProduct(ProductDto product);
    public Task DeleteProduct(Guid productId);
}