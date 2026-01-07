namespace EfCoreApiExample.src.DTOs;

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string Email,
    string Address
);