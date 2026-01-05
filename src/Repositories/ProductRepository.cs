using EfCoreApiTemplate.src.Data;
using EfCoreApiTemplate.src.Entities;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiTemplate.src.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public async Task<Product> CreateProductAsync(Product product) => (await dbContext.Products.AddAsync(product)).Entity;

    public async Task<Product?> GetProductByIdAsync(Guid productId) => await dbContext.Products.FindAsync(productId);
    public async Task<ICollection<Product>> GetProductsByIdAsync(ICollection<Guid> productIds) => await dbContext.Products.Where(product => productIds.Contains(product.Id)).ToListAsync();
    public async Task<Product?> GetProductByNameAsync(string productName) => await dbContext.Products.FirstOrDefaultAsync(product => product.Name == productName);

    public Product UpdateProduct(Product product) => dbContext.Products.Update(product).Entity;

    public void DeleteProduct(Product product) => dbContext.Products.Remove(product);

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();


    public async Task<bool> ProductExistsByIdAsync(Guid productId) => await dbContext.Products.AnyAsync(product => product.Id == productId);

    public async Task<bool> ProductExistsByNameAsync(string productName) => await dbContext.Products.AnyAsync(product => product.Name == productName);
}