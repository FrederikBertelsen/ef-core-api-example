using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Extensions;

public static class DtoValidationExtensions
{
    #region Customer Validations
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
    #endregion

    #region Product Validations
    public static void ValidateOrThrow(this CreateProductDto createProductDto)
    {
        ArgumentNullException.ThrowIfNull(createProductDto);
        if (string.IsNullOrWhiteSpace(createProductDto.Name))
            throw new ArgumentException("The Product is missing a Name");
        if (createProductDto.Price <= 0)
            throw new ArgumentException($"The Product's price must be over 0, got '{createProductDto.Price}'");
    }

    public static void ValidateOrThrow(this ProductDto productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);
        if (productDto.Id == Guid.Empty)
            throw new ArgumentException("The Product is missing an ID");
        if (string.IsNullOrWhiteSpace(productDto.Name))
            throw new ArgumentException("The Product is missing a Name");
        if (productDto.Price is null)
            throw new ArgumentException("The Product is missing a Price");
        if (productDto.Price <= 0)
            throw new ArgumentException($"The Product's price must be over 0, got '{productDto.Price}'");
    }
    #endregion

    #region Order Validations
    public static void ValidateOrThrow(this CreateOrderDto createOrderDto)
    {
        ArgumentNullException.ThrowIfNull(createOrderDto);
        ArgumentNullException.ThrowIfNull(createOrderDto.OrderItemDtos);
        if (createOrderDto.CustomerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty");
        if (createOrderDto.OrderItemDtos.Count == 0)
            throw new ArgumentException("At least one order item must be provided to create an order");
        if (createOrderDto.OrderItemDtos.Any(createOrderItemDto => createOrderItemDto.ProductId == Guid.Empty))
            throw new ArgumentException("OrderItems' ProductId cannot be empty");
        if (createOrderDto.OrderItemDtos.Any(createOrderItemDto => createOrderItemDto.Quantity <= 0))
            throw new ArgumentException("OrderItems must have a quantity higher than zero");

        // check for duplicate product IDs in order items
        var duplicateProductIds = createOrderDto.OrderItemDtos
            .GroupBy(oi => oi.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (duplicateProductIds.Count > 0)
            throw new ArgumentException($"Duplicate ProductIds found in order items: {string.Join(", ", duplicateProductIds)}");
    }

    public static void ValidateOrThrow(this OrderItemDto orderItemDto)
    {
        ArgumentNullException.ThrowIfNull(orderItemDto);
        if (orderItemDto.Product.Id == Guid.Empty)
            throw new ArgumentException("OrderItem's ProductId cannot be empty");
        if (orderItemDto.Quantity <= 0)
            throw new ArgumentException("OrderItem must have a quantity higher than zero");
    }

    public static void ValidateOrThrow(this ICollection<OrderItemDto> orderItemDtos)
    {
        ArgumentNullException.ThrowIfNull(orderItemDtos);

        foreach (var orderItemDto in orderItemDtos)
            orderItemDto.ValidateOrThrow();
    }


    #endregion
}