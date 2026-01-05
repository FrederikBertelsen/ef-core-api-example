using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiTemplate.src.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto newCustomerDto)
    {
        var customerDto = await customerService.CreateCustomerAsync(newCustomerDto);
        return Ok(customerDto);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid customerId)
    {
        var customerDto = await customerService.GetCustomerByIdAsync(customerId);
        return Ok(customerDto);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerDto>> PatchCustomer(CustomerDto customerDto)
    {
        var updatedCustomer = await customerService.UpdateCustomerAsync(customerDto);
        return Ok(updatedCustomer);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCustomer(Guid customerId)
    {
        await customerService.DeleteCustomerAsync(customerId);
        return NoContent();
    }

}