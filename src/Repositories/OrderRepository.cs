using EfCoreApiTemplate.src.Data;
using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiTemplate.src.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        createOrderDto.ValidateOrThrow();

        var customer = await dbContext.Customers.FindAsync(createOrderDto.CustomerId);
        if (customer is null)
            throw new ArgumentException($"No customer found with ID '{createOrderDto.CustomerId}'");

        var productIds = createOrderDto.OrderItemDtos.Select(orderItemDto => orderItemDto.ProductId).ToList();
        var products = await dbContext.Products
            .Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id);

        var missing = productIds.Except(products.Keys).FirstOrDefault();
        if (missing != default)
            throw new ArgumentException($"No product found with ID '{missing}'");

        var orderItems = createOrderDto.OrderItemDtos
            .Select(orderItemDto => new OrderItem
            {
                Product = products[orderItemDto.ProductId],
                Quantity = orderItemDto.Quantity
            })
            .ToList();

        var order = (await dbContext.Orders.AddAsync(new Order
        {
            CustomerId = customer.Id,
            Customer = customer,
            OrderItems = orderItems
        })).Entity;

        await dbContext.SaveChangesAsync();

        return order.ToDto();
    }

    public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        var order = await dbContext.Orders
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .FirstOrDefaultAsync(order => order.Id == orderId);
        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        return order.ToDto();
    }

    public async Task AddProductsToOrderAsync(Guid orderId, ICollection<OrderItemDto> productsToAdd)
    {
        productsToAdd.ValidateOrThrow();

        var order = await GetOrderAndValidate(orderId, includeItems: true);

        // add product, or increase quantity if it already exists
        foreach (var productDto in productsToAdd)
        {
            var existingItem = order?.OrderItems.FirstOrDefault(orderItem => orderItem.Product.Id == productDto.Product.Id);
            if (existingItem != null)
                existingItem.Quantity += productDto.Quantity;
            else
            {
                var product = await dbContext.Products.FindAsync(productDto.Product.Id);
                if (product != null)
                {
                    order?.OrderItems.Add(new OrderItem
                    {
                        Product = product,
                        Quantity = productDto.Quantity
                    });
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveProductsFromOrderAsync(Guid orderId, ICollection<OrderItemDto> productsToRemove)
    {
        productsToRemove.ValidateOrThrow();

        var order = await GetOrderAndValidate(orderId, includeItems: true);

        // decrease quantity, or remove item if it exists or quantity reaches zero
        foreach (var productDto in productsToRemove)
        {
            var existingItem = order?.OrderItems.FirstOrDefault(orderItem => orderItem.Product.Id == productDto.Product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity -= productDto.Quantity;

                if (existingItem.Quantity <= 0)
                    order?.OrderItems.Remove(existingItem);
            }
            else
                throw new ArgumentException($"Product with ID '{productDto.Product.Id}' not found in order '{orderId}'");
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task MarkOrderAsCompletedAsync(Guid orderId)
    {
        var order = await GetOrderAndValidate(orderId);

        order.MarkAsCompleted();

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        var order = await GetOrderAndValidate(orderId);

        dbContext.Orders.Remove(order);

        await dbContext.SaveChangesAsync();
    }

    private async Task<Order> GetOrderAndValidate(Guid orderId, bool includeItems = false)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        Order? order;
        if (includeItems)
            order = await dbContext.Orders
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .FirstOrDefaultAsync(order => order.Id == orderId);
        else
            order = await dbContext.Orders.FindAsync(orderId);

        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        return order;
    }
}