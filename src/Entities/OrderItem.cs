namespace EfCoreApiExample.src.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; init; }
    public required Order Order { get; init; }
    public required Product Product { get; init; }
    public required int Quantity { get; set; }
}