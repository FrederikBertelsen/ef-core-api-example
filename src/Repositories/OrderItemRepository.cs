using EfCoreApiExample.src.Data;
using EfCoreApiExample.src.Entities;
using EfCoreApiExample.src.Repositories.Interfaces;

namespace EfCoreApiExample.src.Repositories;

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