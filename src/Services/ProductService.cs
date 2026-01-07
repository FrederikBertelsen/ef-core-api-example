using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Exceptions;
using EfCoreApiExample.src.Extensions;
using EfCoreApiExample.src.Repositories.Interfaces;
using EfCoreApiExample.src.Services.Interfaces;

namespace EfCoreApiExample.src.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        createProductDto.ValidateOrThrow();

        if (await productRepository.ProductExistsByNameAsync(createProductDto.Name))
            throw new AlreadyExistsException("Product", "Name", createProductDto.Name);

        var product = createProductDto.ToEntity();

        var entity = await productRepository.CreateProductAsync(product);
        await productRepository.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new MissingValueException("Product Id");

        var product = await productRepository.GetProductByIdAsync(productId);
        if (product is null)
            throw new NotFoundException("Product", "Id", productId);

        return product.ToDto();
    }

    public async Task<ProductDto> GetProductByNameAsync(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new MissingValueException("Product Name");

        var product = await productRepository.GetProductByNameAsync(productName);
        if (product is null)
            throw new NotFoundException("Product", "Name", productName);

        return product.ToDto();
    }

    public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
    {
        if (productDto is null)
            throw new MissingValueException("Product data");
        if (productDto.Id == Guid.Empty)
            throw new MissingValueException("Product", nameof(productDto.Id));
        if (productDto.Name is not null && productDto.Name.Trim().Length == 0)
            throw new InvalidValueException(nameof(productDto.Name), "non-empty");
        if (productDto.Price is not null && productDto.Price <= 0)
            throw new InvalidValueException(nameof(productDto.Price), "greater than 0", productDto.Price);

        var product = await productRepository.GetProductByIdAsync(productDto.Id);
        if (product is null)
            throw new NotFoundException("Product", "Id", productDto.Id);

        // update Entity field only if DTO field is not null
        product.ApplyNonNullValues(productDto);

        var updatedProduct = productRepository.UpdateProduct(product);
        await productRepository.SaveChangesAsync();

        return updatedProduct.ToDto();
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new MissingValueException("Product Id");

        var product = await productRepository.GetProductByIdAsync(productId);
        if (product is null)
            throw new NotFoundException("Product", "Id", productId);

        productRepository.DeleteProduct(product);

        await productRepository.SaveChangesAsync();
    }
}