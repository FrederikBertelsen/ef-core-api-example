using EfCoreApiTemplate.src.DTOs;

interface IOrderRepository
{
    public Task<OrderDto> CreateOrder(CreateOrderDto order);
    public Task AddProductsToOrder(Guid orderId, ICollection<Guid> productIds);
    public Task RemoveProductsFromOrder(Guid orderId, ICollection<Guid> productIds);
    public Task MarkOrderAsComplete(Guid orderId);
    public Task DeleteOrder(Guid OrderId);
}