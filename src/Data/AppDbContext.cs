using EfCoreApiExample.src.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiExample.src.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; init; }
    public DbSet<Order> Orders { get; init; }
    public DbSet<OrderItem> OrderItems { get; init; }
    public DbSet<Product> Products { get; init; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customer <=> Order
        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Orders)
            .WithOne(order => order.Customer)
            .HasForeignKey(order => order.CustomerId);

        // Order => OrderItems
        modelBuilder.Entity<Order>()
            .HasMany(order => order.OrderItems)
            .WithOne(orderItem => orderItem.Order)
            .HasForeignKey(orderItem => orderItem.OrderId);

        // OrderItems => Product
        modelBuilder.Entity<OrderItem>()
            .HasOne(orderItem => orderItem.Product);
    }
}