using EfCoreApiTemplate.src.DTOs;

interface IProductRepository
{
    public Task<ProductDto> CreateProduct(CreateProductDto product);
    public Task<ProductDto> PatchProduct(ProductDto product);
    public Task DeleteProduct(Guid productId);
}