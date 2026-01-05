using EfCoreApiTemplate.src.Data;
using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiTemplate.src.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public async Task<Order> CreateOrderAsync(Order order) => (await dbContext.Orders.AddAsync(order)).Entity;

    public async Task<Order?> GetOrderByIdAsync(Guid orderId) => await dbContext.Orders
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .FirstOrDefaultAsync(order => order.Id == orderId);

    public void DeleteOrder(Order order) => dbContext.Orders.Remove(order);

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}