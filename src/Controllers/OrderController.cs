using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiTemplate.src.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var orderDto = await orderRepository.CreateOrderAsync(createOrderDto);
        return Ok(orderDto);
    }

    [HttpGet]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
    {
        var orderDto = await orderRepository.GetOrderByIdAsync(orderId);
        return Ok(orderDto);
    }

    [HttpPut("AddProducts")]
    public async Task<IActionResult> AddProductsToOrder(Guid orderId, ICollection<OrderItemDto> productsToAdd)
    {
        await orderRepository.AddProductsToOrderAsync(orderId, productsToAdd);
        return NoContent();
    }

    [HttpPut("RemoveProducts")]
    public async Task<IActionResult> RemoveProductsFromOrder(Guid orderId, ICollection<OrderItemDto> productsToRemove)
    {
        await orderRepository.RemoveProductsFromOrderAsync(orderId, productsToRemove);
        return NoContent();
    }

    [HttpPut("MarkAsCompleted")]
    public async Task<IActionResult> MarkOrderAsCompleted(Guid orderId)
    {
        await orderRepository.MarkOrderAsCompletedAsync(orderId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        await orderRepository.DeleteOrderAsync(orderId);
        return NoContent();
    }
}