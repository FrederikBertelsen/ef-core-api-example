using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Extensions;

public static class CustomerValidationExtensions
{
    public static void ValidateOrThrow(this CreateCustomerDto createCustomerDto)
    {
        ArgumentNullException.ThrowIfNull(createCustomerDto);
        if (string.IsNullOrWhiteSpace(createCustomerDto.FirstName))
            throw new ArgumentException("The Customer is missing a first name");
        if (string.IsNullOrWhiteSpace(createCustomerDto.LastName))
            throw new ArgumentException("The Customer is missing a last name");
        if (string.IsNullOrWhiteSpace(createCustomerDto.Email))
            throw new ArgumentException("The Customer is missing an email");
        if (string.IsNullOrWhiteSpace(createCustomerDto.Address))
            throw new ArgumentException("The Customer is missing an address");
    }

    public static void ValidateOrThrow(this CustomerDto CustomerDto)
    {
        ArgumentNullException.ThrowIfNull(CustomerDto);
        if (CustomerDto.Id == Guid.Empty)
            throw new ArgumentException("The Customer is missing an Id");
        if (string.IsNullOrWhiteSpace(CustomerDto.FirstName))
            throw new ArgumentException("The Customer is missing a first name");
        if (string.IsNullOrWhiteSpace(CustomerDto.LastName))
            throw new ArgumentException("The Customer is missing a last name");
        if (string.IsNullOrWhiteSpace(CustomerDto.Email))
            throw new ArgumentException("The Customer is missing an email");
        if (string.IsNullOrWhiteSpace(CustomerDto.Address))
            throw new ArgumentException("The Customer is missing an address");
    }
}