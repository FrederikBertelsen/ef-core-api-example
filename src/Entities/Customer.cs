using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreApiTemplate.src.Entities;

public class Customer : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public ICollection<Order> Orders { get; init; } = [];

    [NotMapped]
    public string FullName => FirstName + " " + LastName;
}