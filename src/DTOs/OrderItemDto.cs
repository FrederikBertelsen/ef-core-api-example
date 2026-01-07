namespace EfCoreApiExample.src.DTOs;

public record OrderItemDto(
    ProductDto Product,
    int Quantity
);