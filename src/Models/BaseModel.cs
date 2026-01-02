namespace EfCoreApiTemplate.src.Models;

public abstract class BaseModel
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}