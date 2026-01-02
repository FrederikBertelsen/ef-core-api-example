using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiTemplate.src.Controllers;

[Route("api/Products")]
[ApiController]
public class ProductController(IProductRepository productRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto newProductDto)
    {
        var productDto = await productRepository.CreateProduct(newProductDto);
        return Ok(productDto);
    }

    [HttpPatch("{productId:guid}/price/{newPrice:float}")]
    public async Task<ActionResult<ProductDto>> UpdatePrice(Guid productId, float newPrice)
    {
        var updatedProduct = await productRepository.UpdatePrice(productId, newPrice);
        return Ok(updatedProduct);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        await productRepository.DeleteProduct(productId);
        return NoContent();
    }
}