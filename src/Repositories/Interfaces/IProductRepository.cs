using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<Product> CreateProductAsync(Product product);
    public Task<Product?> GetProductByIdAsync(Guid productId);
    public Task<ICollection<Product>> GetProductsByIdAsync(ICollection<Guid> productIds);
    public Task<Product?> GetProductByNameAsync(string productName);
    public Product UpdateProduct(Product product);
    public void DeleteProduct(Product product);
    public Task<bool> ProductExistsByIdAsync(Guid productId);
    public Task<bool> ProductExistsByNameAsync(string productName);
    public Task SaveChangesAsync();
}