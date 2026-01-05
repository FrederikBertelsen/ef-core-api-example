using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiTemplate.src.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var orderDto = await orderService.CreateOrderAsync(createOrderDto);
        return Ok(orderDto);
    }

    [HttpGet]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
    {
        var orderDto = await orderService.GetOrderByIdAsync(orderId);
        return Ok(orderDto);
    }

    [HttpPut("AddProducts")]
    public async Task<IActionResult> AddProductsToOrder(Guid orderId, ICollection<OrderItemDto> productsToAdd)
    {
        await orderService.AddProductsToOrderAsync(orderId, productsToAdd);
        return NoContent();
    }

    [HttpPut("RemoveProducts")]
    public async Task<IActionResult> RemoveProductsFromOrder(Guid orderId, ICollection<OrderItemDto> productsToRemove)
    {
        await orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove);
        return NoContent();
    }

    [HttpPut("MarkAsCompleted")]
    public async Task<IActionResult> MarkOrderAsCompleted(Guid orderId)
    {
        await orderService.MarkOrderAsCompletedAsync(orderId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        await orderService.DeleteOrderAsync(orderId);
        return NoContent();
    }
}