using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<OrderDto> CreateOrder(CreateOrderDto createOrderDto);
    public Task AddProductsToOrder(Guid orderId, ICollection<Guid> productIds);
    public Task RemoveProductsFromOrder(Guid orderId, ICollection<Guid> productIds);
    public Task MarkOrderAsComplete(Guid orderId);
    public Task DeleteOrder(Guid orderId);
}