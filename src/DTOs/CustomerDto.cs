namespace EfCoreApiExample.src.DTOs;

public record CustomerDto(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Address
);