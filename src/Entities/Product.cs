namespace EfCoreApiTemplate.src.Entities;

public class Product : BaseEntity
{
    public required string Name { get; init; }
    public required float Price { get; set; }
}