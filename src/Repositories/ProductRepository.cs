using EfCoreApiTemplate.src.Data;
using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiTemplate.src.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        createProductDto.ValidateOrThrow();

        if (await dbContext.Products.AnyAsync(product => product.Name == createProductDto.Name))
            throw new ArgumentException($"A Product with Name '{createProductDto.Name}' already exists");

        var entity = (await dbContext.Products.AddAsync(createProductDto.ToEntity())).Entity;
        await dbContext.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("The Product ID cannot be empty");

        var product = await dbContext.Products.FindAsync(productId);

        if (product is null)
            throw new ArgumentException($"No Product found with ID '{productId}'");

        return product.ToDto();
    }

    public async Task<ProductDto> UpdatePriceAsync(Guid productId, float newPrice)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("The Product ID cannot be empty");

        if (newPrice <= 0)
            throw new ArgumentException($"The Product's price must be over 0, got '{newPrice}'");

        var product = await dbContext.Products.FindAsync(productId);
        if (product is null)
            throw new ArgumentException($"No Product found with ID '{productId}'");

        product.Price = newPrice;
        await dbContext.SaveChangesAsync();

        return product.ToDto();
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("The Product ID cannot be empty");

        var product = await dbContext.Products.FindAsync(productId);
        if (product is null)
            throw new ArgumentException($"No Product found with ID '{productId}'");

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}