namespace EfCoreApiTemplate.src.DTOs;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    ICollection<ProductDto> Products,
    DateTime CreatedAt
);