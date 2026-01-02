using EfCoreApiTemplate.src.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiTemplate.src.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<Order> Orders { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customer <=> Order
        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Orders)
            .WithOne(order => order.Customer)
            .HasForeignKey(order => order.CustomerId);

        // Order => Product
        modelBuilder.Entity<Order>()
            .HasMany(order => order.Products);
    }
}