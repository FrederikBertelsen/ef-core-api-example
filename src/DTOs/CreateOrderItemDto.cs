namespace EfCoreApiTemplate.src.DTOs;

public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity
);