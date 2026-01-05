using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order?> GetOrderByIdAsync(Guid orderId);
    public void DeleteOrder(Order order);
    public Task SaveChangesAsync();
}