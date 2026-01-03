namespace EfCoreApiTemplate.src.DTOs;

public record CreateOrderDto(
    Guid CustomerId,
    ICollection<CreateOrderItemDto> OrderItemDtos
);