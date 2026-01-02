using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreApiTemplate.src.Entities;

public class Customer : BaseEntity
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public ICollection<Order> Orders { get; init; } = [];

    [NotMapped]
    public string FullName => FirstName + " " + LastName;
}