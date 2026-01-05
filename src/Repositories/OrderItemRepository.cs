using EfCoreApiTemplate.src.Data;
using EfCoreApiTemplate.src.Entities;
using EfCoreApiTemplate.src.Repositories.Interfaces;

namespace EfCoreApiTemplate.src.Repositories;

public class OrderItemRepository(AppDbContext dbContext) : IOrderItemRepository
{
    public async Task CreateOrderItemsAsync(ICollection<OrderItem> orderItems)
    {
        foreach (var orderItem in orderItems)
            await dbContext.OrderItems.AddAsync(orderItem);
    }

    public void DeleteOrderItems(ICollection<OrderItem> orderItems)
    {
        foreach (var orderItem in orderItems)
            dbContext.OrderItems.Remove(orderItem);
    }

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}