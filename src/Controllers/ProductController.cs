using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiExample.src.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto newProductDto)
    {
        var productDto = await productService.CreateProductAsync(newProductDto);
        return Ok(productDto);
    }

    [HttpGet]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid productId)
    {
        var productDto = await productService.GetProductByIdAsync(productId);
        return Ok(productDto);
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> UpdateProduct(ProductDto productDto)
    {
        var updatedProduct = await productService.UpdateProductAsync(productDto);
        return Ok(updatedProduct);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        await productService.DeleteProductAsync(productId);
        return NoContent();
    }
}