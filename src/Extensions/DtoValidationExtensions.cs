using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Extensions;

public static class DtoValidationExtensions
{
    public static void ValidateOrThrow(this CreateCustomerDto createCustomerDto)
    {
        ArgumentNullException.ThrowIfNull(createCustomerDto);
        if (string.IsNullOrWhiteSpace(createCustomerDto.FirstName))
            throw new ArgumentException("The Customer is missing a FirstName");
        if (string.IsNullOrWhiteSpace(createCustomerDto.LastName))
            throw new ArgumentException("The Customer is missing a LastName");
        if (string.IsNullOrWhiteSpace(createCustomerDto.Email))
            throw new ArgumentException("The Customer is missing an Email");
        if (string.IsNullOrWhiteSpace(createCustomerDto.Address))
            throw new ArgumentException("The Customer is missing an Address");
    }

    public static void ValidateOrThrow(this CustomerDto CustomerDto)
    {
        ArgumentNullException.ThrowIfNull(CustomerDto);
        if (CustomerDto.Id == Guid.Empty)
            throw new ArgumentException("The Customer is missing an ID");
        if (string.IsNullOrWhiteSpace(CustomerDto.FirstName))
            throw new ArgumentException("The Customer is missing a FirstName");
        if (string.IsNullOrWhiteSpace(CustomerDto.LastName))
            throw new ArgumentException("The Customer is missing a LastName");
        if (string.IsNullOrWhiteSpace(CustomerDto.Email))
            throw new ArgumentException("The Customer is missing an Email");
        if (string.IsNullOrWhiteSpace(CustomerDto.Address))
            throw new ArgumentException("The Customer is missing an Address");
    }
}