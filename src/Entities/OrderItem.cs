namespace EfCoreApiTemplate.src.Entities;

public class OrderItem
{
    public required Product Product { get; init; }
    public required int Quantity { get; set; }
}