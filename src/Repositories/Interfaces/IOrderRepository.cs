using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Entities;

namespace EfCoreApiExample.src.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order?> GetOrderByIdAsync(Guid orderId);
    public void DeleteOrder(Order order);
    public Task SaveChangesAsync();
}