using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using EfCoreApiTemplate.src.Services.Interfaces;

namespace EfCoreApiTemplate.src.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        createProductDto.ValidateOrThrow();

        if (await productRepository.ProductExistsByNameAsync(createProductDto.Name))
            throw new ArgumentException($"A Product with Name '{createProductDto.Name}' already exists");

        var product = createProductDto.ToEntity();

        var entity = await productRepository.CreateProductAsync(product);
        await productRepository.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("The Product ID cannot be empty");

        var product = await productRepository.GetProductByIdAsync(productId);
        if (product is null)
            throw new ArgumentException($"No Product found with ID '{productId}'");

        return product.ToDto();
    }

    public async Task<ProductDto> GetProductByNameAsync(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("The Product Name cannot be empty");

        var product = await productRepository.GetProductByNameAsync(productName);
        if (product is null)
            throw new ArgumentException($"No Product found with Name '{productName}'");

        return product.ToDto();
    }

    public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);
        if (productDto.Id == Guid.Empty)
            throw new ArgumentException("The Product is missing an ID");
        if (productDto.Name is not null && productDto.Name.Trim().Length == 0)
            throw new ArgumentException("The Product Name cannot be empty");
        if (productDto.Price is not null && productDto.Price <= 0)
            throw new ArgumentException($"The Product's price must be over 0, got '{productDto.Price}'");

        var product = await productRepository.GetProductByIdAsync(productDto.Id);
        if (product is null)
            throw new ArgumentException($"No Product found with ID '{productDto.Id}'");

        // update Entity field only if DTO field is not null
        product.ApplyNonNullValues(productDto);

        var updatedProduct = productRepository.UpdateProduct(product);
        await productRepository.SaveChangesAsync();

        return updatedProduct.ToDto();
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("The Product ID cannot be empty");

        var product = await productRepository.GetProductByIdAsync(productId);
        if (product is null)
            throw new ArgumentException($"No Product found with ID '{productId}'");

        productRepository.DeleteProduct(product);

        await productRepository.SaveChangesAsync();
    }
}