using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Exceptions;

namespace EfCoreApiExample.src.Extensions;

public static class DtoValidationExtensions
{
    #region Customer Validations
    public static void ValidateOrThrow(this CreateCustomerDto createCustomerDto)
    {
        if (createCustomerDto is null)
            throw new MissingValueException("Customer data");
        if (string.IsNullOrWhiteSpace(createCustomerDto.FirstName))
            throw new MissingValueException("Customer", nameof(createCustomerDto.FirstName));
        if (string.IsNullOrWhiteSpace(createCustomerDto.LastName))
            throw new MissingValueException("Customer", nameof(createCustomerDto.LastName));
        if (string.IsNullOrWhiteSpace(createCustomerDto.Email))
            throw new MissingValueException("Customer", nameof(createCustomerDto.Email));
        if (string.IsNullOrWhiteSpace(createCustomerDto.Address))
            throw new MissingValueException("Customer", nameof(createCustomerDto.Address));
    }

    public static void ValidateOrThrow(this CustomerDto CustomerDto)
    {
        if (CustomerDto is null)
            throw new MissingValueException("Customer data");
        if (CustomerDto.Id == Guid.Empty)
            throw new MissingValueException("Customer", nameof(CustomerDto.Id));
        if (string.IsNullOrWhiteSpace(CustomerDto.FirstName))
            throw new MissingValueException("Customer", nameof(CustomerDto.FirstName));
        if (string.IsNullOrWhiteSpace(CustomerDto.LastName))
            throw new MissingValueException("Customer", nameof(CustomerDto.LastName));
        if (string.IsNullOrWhiteSpace(CustomerDto.Email))
            throw new MissingValueException("Customer", nameof(CustomerDto.Email));
        if (string.IsNullOrWhiteSpace(CustomerDto.Address))
            throw new MissingValueException("Customer", nameof(CustomerDto.Address));
    }
    #endregion

    #region Product Validations
    public static void ValidateOrThrow(this CreateProductDto createProductDto)
    {
        if (createProductDto is null)
            throw new MissingValueException("Product data");
        if (string.IsNullOrWhiteSpace(createProductDto.Name))
            throw new MissingValueException("Product", nameof(createProductDto.Name));
        if (createProductDto.Price <= 0)
            throw new InvalidValueException(nameof(createProductDto.Price), "greater than 0", createProductDto.Price);
    }

    public static void ValidateOrThrow(this ProductDto productDto)
    {
        if (productDto is null)
            throw new MissingValueException("Product data");
        if (productDto.Id == Guid.Empty)
            throw new MissingValueException("Product", nameof(productDto.Id));
        if (string.IsNullOrWhiteSpace(productDto.Name))
            throw new MissingValueException("Product", nameof(productDto.Name));
        if (productDto.Price is null)
            throw new MissingValueException("Product", nameof(productDto.Price));
        if (productDto.Price <= 0)
            throw new InvalidValueException(nameof(productDto.Price), "greater than 0", productDto.Price);
    }
    #endregion

    #region Order Validations
    public static void ValidateOrThrow(this CreateOrderDto createOrderDto)
    {
        if (createOrderDto is null)
            throw new MissingValueException("Order data");
        if (createOrderDto.OrderItemDtos is null)
            throw new MissingValueException("Order", nameof(createOrderDto.OrderItemDtos));
        if (createOrderDto.CustomerId == Guid.Empty)
            throw new MissingValueException("Order", nameof(createOrderDto.CustomerId));
        if (createOrderDto.OrderItemDtos.Count == 0)
            throw new InvalidValueException(nameof(createOrderDto.OrderItemDtos), "at least one item");
        if (createOrderDto.OrderItemDtos.Any(item => item.ProductId == Guid.Empty))
            throw new MissingValueException("Order Item", nameof(CreateOrderItemDto.ProductId));
        if (createOrderDto.OrderItemDtos.Any(item => item.Quantity <= 0))
            throw new InvalidValueException(nameof(CreateOrderItemDto.Quantity), "greater than 0");

        // check for duplicate product IDs in order items
        var duplicateProductIds = createOrderDto.OrderItemDtos
            .GroupBy(oi => oi.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (duplicateProductIds.Count > 0)
            throw new InvalidValueException(nameof(createOrderDto.OrderItemDtos), $"unique ProductIds. Found duplicates: {string.Join(", ", duplicateProductIds)}");
    }

    public static void ValidateOrThrow(this OrderItemDto orderItemDto)
    {
        if (orderItemDto is null)
            throw new MissingValueException("Order Item data");
        if (orderItemDto.Product.Id == Guid.Empty)
            throw new MissingValueException("Order Item", nameof(orderItemDto.Product.Id));
        if (orderItemDto.Quantity <= 0)
            throw new InvalidValueException(nameof(orderItemDto.Quantity), "greater than 0");
    }

    public static void ValidateOrThrow(this ICollection<OrderItemDto> orderItemDtos)
    {
        if (orderItemDtos is null)
            throw new MissingValueException("Order Items");

        foreach (var orderItemDto in orderItemDtos)
            orderItemDto.ValidateOrThrow();
    }


    #endregion
}