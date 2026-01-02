public record CreateOrderDto(
    Guid CustomerId,
    ICollection<Guid> productIds
);