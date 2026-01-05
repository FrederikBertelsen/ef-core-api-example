using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using EfCoreApiTemplate.src.Services.Interfaces;

namespace EfCoreApiTemplate.src.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IOrderItemRepository orderItemRepository,
    IProductRepository productRepository,
    ICustomerRepository customerRepository) : IOrderService
{
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        createOrderDto.ValidateOrThrow();

        var customer = await customerRepository.GetCustomerByIdAsync(createOrderDto.CustomerId);
        if (customer is null)
            throw new ArgumentException($"No customer found with ID '{createOrderDto.CustomerId}'");

        var productIds = createOrderDto.OrderItemDtos.Select(orderItemDto => orderItemDto.ProductId).ToList();
        var products = (await productRepository.GetProductsByIdAsync(productIds)).ToDictionary(product => product.Id);

        var missing = productIds.Except(products.Keys).FirstOrDefault();
        if (missing != default)
            throw new ArgumentException($"No product found with ID '{missing}'");


        var order = new Order
        {
            CustomerId = customer.Id,
            Customer = customer,

            OrderItems = createOrderDto.OrderItemDtos
            .Select(orderItemDto => new OrderItem
            {
                Product = products[orderItemDto.ProductId],
                Quantity = orderItemDto.Quantity
            })
            .ToList()
        };
        var entity = await orderRepository.CreateOrderAsync(order);
        await orderRepository.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        return order.ToDto();
    }
    public async Task AddProductsToOrderAsync(Guid orderId, ICollection<OrderItemDto> productsToAdd)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        productsToAdd.ValidateOrThrow();

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        // add new OrderItem, or increase quantity if it already exists
        var newOrderItems = new List<OrderItem>();
        foreach (var productDto in productsToAdd)
        {
            var existingItem = order.OrderItems.FirstOrDefault(orderItem => orderItem.Product.Id == productDto.Product.Id);
            if (existingItem != null)
                existingItem.Quantity += productDto.Quantity;
            else
            {
                var product = await productRepository.GetProductByIdAsync(productDto.Product.Id);
                if (product != null)
                {
                    newOrderItems.Add(new OrderItem
                    {
                        Order = order,
                        OrderId = order.Id,
                        Product = product,
                        Quantity = productDto.Quantity
                    });
                }
            }
        }

        await orderItemRepository.CreateOrderItemsAsync(newOrderItems);
        await orderItemRepository.SaveChangesAsync();
    }

    public async Task RemoveProductsFromOrderAsync(Guid orderId, ICollection<OrderItemDto> productsToRemove)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        productsToRemove.ValidateOrThrow();

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        // decrease quantity or remove OrderItem
        var orderItemsToRemove = new List<OrderItem>();
        foreach (var productDto in productsToRemove)
        {
            var existingItem = order.OrderItems.FirstOrDefault(orderItem => orderItem.Product.Id == productDto.Product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity -= productDto.Quantity;
                if (existingItem.Quantity <= 0)
                    orderItemsToRemove.Add(existingItem);
            }
        }

        orderItemRepository.DeleteOrderItems(orderItemsToRemove);
        await orderItemRepository.SaveChangesAsync();
    }

    public async Task MarkOrderAsCompletedAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        order.MarkAsCompleted();
        await orderRepository.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("orderId cannot be empty");

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order is null)
            throw new ArgumentException($"No order found with ID '{orderId}'");

        orderRepository.DeleteOrder(order);
        await orderRepository.SaveChangesAsync();
    }
}