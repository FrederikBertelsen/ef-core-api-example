using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApiTemplate.src.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ICustomerRepository customerRepository) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto newCustomerDto)
    {
        var customerDto = await customerRepository.CreateCustomerAsync(newCustomerDto);
        return Ok(customerDto);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid customerId)
    {
        var customerDto = await customerRepository.GetCustomerByIdAsync(customerId);
        return Ok(customerDto);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerDto>> PatchCustomer(CustomerDto customerDto)
    {
        var updatedCustomer = await customerRepository.UpdateCustomerAsync(customerDto);
        return Ok(updatedCustomer);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCustomer(Guid customerId)
    {
        await customerRepository.DeleteCustomerAsync(customerId);
        return NoContent();
    }

}