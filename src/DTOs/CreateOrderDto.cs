namespace EfCoreApiExample.src.DTOs;

public record CreateOrderDto(
    Guid CustomerId,
    ICollection<CreateOrderItemDto> OrderItemDtos
);