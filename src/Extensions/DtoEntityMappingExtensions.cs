using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Entities;

namespace EfCoreApiTemplate.src.Extensions;

public static class EntityDtoMappingExtensions
{
    #region Customer Mappings
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

    #endregion

    #region Product Mappings
    public static Product ToEntity(this CreateProductDto createProductDto)
    {
        createProductDto.ValidateOrThrow();
        return new()
        {
            Name = createProductDto.Name!,
            Price = createProductDto.Price,
        };
    }
    public static Product ToEntity(this ProductDto productDto)
    {
        productDto.ValidateOrThrow();

        return new()
        {
            Id = productDto.Id,
            Name = productDto.Name!,
            Price = (float)productDto.Price!,
        };
    }

    public static ProductDto ToDto(this Product product)
    {
        return new(
            Id: product.Id,
            Name: product.Name,
            Price: product.Price
        );
    }
    #endregion
}