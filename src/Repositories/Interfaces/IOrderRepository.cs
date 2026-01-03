using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<OrderDto> CreateOrder(CreateOrderDto createOrderDto);
    public Task AddProductsToOrder(Guid orderId, ICollection<OrderItemDto> productsToAdd);
    public Task RemoveProductsFromOrder(Guid orderId, ICollection<OrderItemDto> productsToRemove);
    public Task MarkOrderAsComplete(Guid orderId);
    public Task DeleteOrder(Guid orderId);
}