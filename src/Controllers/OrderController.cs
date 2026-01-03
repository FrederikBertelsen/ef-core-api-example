using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiTemplate.src.Controllers;

[Route("api/Orders")]
[ApiController]
public class OrderController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var orderDto =  await orderRepository.CreateOrder(createOrderDto);
        return Ok(orderDto);
    }

    [HttpPatch("AddProducts")]
    public async Task<IActionResult> AddProductsToOrder(Guid orderId, ICollection<OrderItemDto> productsToAdd)
    {
        await orderRepository.AddProductsToOrder(orderId, productsToAdd);
        return NoContent();
    }

    [HttpPatch("RemoveProducts")]
    public async Task<IActionResult> RemoveProductsFromOrder(Guid orderId, ICollection<OrderItemDto> productsToRemove)
    {
        await orderRepository.RemoveProductsFromOrder(orderId, productsToRemove);
        return NoContent();
    }

    [HttpPatch("MarkAsCompleted")]
    public async Task<IActionResult> MarkOrderAsCompleted(Guid orderId)
    {
        await orderRepository.MarkOrderAsCompleted(orderId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        await orderRepository.DeleteOrder(orderId);
        return NoContent();
    }
}