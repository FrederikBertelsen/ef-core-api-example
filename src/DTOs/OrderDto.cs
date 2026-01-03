namespace EfCoreApiTemplate.src.DTOs;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    ICollection<OrderItemDto> OrderItems,
    DateTime CreatedAt
);