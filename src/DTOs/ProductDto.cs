namespace EfCoreApiExample.src.DTOs;

public record ProductDto(
    Guid Id,
    string? Name,
    float? Price
);