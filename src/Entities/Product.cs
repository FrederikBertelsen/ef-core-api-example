namespace EfCoreApiExample.src.Entities;

public class Product : BaseEntity
{
    public required string? Name { get; set; }
    public required float? Price { get; set; }
}