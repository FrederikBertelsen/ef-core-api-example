using System.ComponentModel.DataAnnotations.Schema;
using EfCoreApiExample.src.Exceptions;

namespace EfCoreApiExample.src.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; init; }
    public required ICollection<OrderItem> OrderItems { get; init; }
    public bool IsCompleted { get; private set; } = false;
    public void MarkAsCompleted()
    {
        if (IsCompleted)
            throw new BusinessLogicException("Order is already marked as complete");

        IsCompleted = true;
    }

    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public float TotalPrice()
    {
        if (OrderItems == null || OrderItems.Count == 0)
            return 0f;

        float total = 0f;
        foreach (var orderItem in OrderItems)
        {
            if (orderItem == null || orderItem.Product == null || orderItem.Product.Price == null)
                continue;

            total += (float)orderItem.Product.Price * orderItem.Quantity;
        }

        return total;
    }
}