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
        var customerDto = await customerRepository.CreateCustomer(newCustomerDto);
        return Ok(customerDto);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid customerId)
    {
        var customerDto = await customerRepository.GetCustomerById(customerId);
        return Ok(customerDto);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerDto>> PatchCustomer(CustomerDto customerDto)
    {
        var updatedCustomer = await customerRepository.PatchCustomer(customerDto);
        return Ok(updatedCustomer);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCustomer(Guid customerId)
    {
        await customerRepository.DeleteCustomer(customerId);
        return NoContent();
    }

}