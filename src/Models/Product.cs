namespace EfCoreApiTemplate.src.Models;

public class Product : BaseModel
{
    public required string Name { get; init; }
    public required float Price { get; init; }
}