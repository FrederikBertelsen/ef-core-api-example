using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreApiTemplate.src.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; init; }
    public required ICollection<Product> Products { get; init; }
    public bool IsCompleted { get; private set; } = false;
    public void MarkAsCompleted()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Order is already marked as complete");

        IsCompleted = true;
    }

    [NotMapped]
    public float TotalPrice => Products.Sum(p => p.Price);
}