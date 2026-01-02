using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreApiTemplate.src.Models;

public class Customer : BaseModel
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public ICollection<Order> Orders { get; init; } = [];

    [NotMapped]
    public string FullName => FirstName + " " + LastName;
}