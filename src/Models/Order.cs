using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreApiTemplate.src.Models;

public class Order : BaseModel
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; init; }
    public required ICollection<Product> Products { get; init; }

    [NotMapped]
    public float TotalPrice => Products.Sum(p => p.Price);
}