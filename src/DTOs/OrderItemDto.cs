namespace EfCoreApiTemplate.src.DTOs;

public record OrderItemDto(
    ProductDto Product,
    int Quantity
);