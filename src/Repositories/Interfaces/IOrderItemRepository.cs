using EfCoreApiTemplate.src.Entities;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IOrderItemRepository
{
    public Task CreateOrderItemsAsync(ICollection<OrderItem> orderItems);
    public void DeleteOrderItems(ICollection<OrderItem> orderItems);
    public Task SaveChangesAsync();
}