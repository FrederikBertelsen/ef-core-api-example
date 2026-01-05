using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Services.Interfaces;

public interface IOrderService
{
    public Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    public Task<OrderDto> GetOrderByIdAsync(Guid orderId);
    public Task AddProductsToOrderAsync(Guid orderId, ICollection<OrderItemDto> productsToAdd);
    public Task RemoveProductsFromOrderAsync(Guid orderId, ICollection<OrderItemDto> productsToRemove);
    public Task MarkOrderAsCompletedAsync(Guid orderId);
    public Task DeleteOrderAsync(Guid orderId);
}