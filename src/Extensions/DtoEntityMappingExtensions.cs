using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;
using EfCoreApiTemplate.src.Extensions;

namespace EfCoreApiTemplate.src.Mapping;

public static class EntityDtoMappingExtensions
{
    public static Customer ToEntity(this CustomerDto customerDto)
    {
        customerDto.ValidateOrThrow();

        return new()
        {
            Id = customerDto.Id,
            FirstName = customerDto.FirstName!,
            LastName = customerDto.LastName!,
            Email = customerDto.Email!,
            Address = customerDto.Address!,
        };
    }

    public static Customer ToEntity(this CreateCustomerDto createCustomerDto)
    {
        createCustomerDto.ValidateOrThrow();

        return new()
        {
            FirstName = createCustomerDto.FirstName!,
            LastName = createCustomerDto.LastName!,
            Email = createCustomerDto.Email!,
            Address = createCustomerDto.Address!,
        };
    }

    public static CustomerDto ToDto(this Customer customer)
    {
        return new(
            Id: customer.Id,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email,
            Address: customer.Address
        );
    }
}