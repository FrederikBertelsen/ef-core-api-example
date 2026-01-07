using EfCoreApiExample.src.Data;
using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Entities;
using EfCoreApiExample.src.Extensions;
using EfCoreApiExample.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiExample.src.Repositories;

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