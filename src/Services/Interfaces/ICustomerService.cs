using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Services.Interfaces;

public interface ICustomerService
{
    public Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    public Task<CustomerDto> GetCustomerByIdAsync(Guid customerId);
    public Task<CustomerDto> UpdateCustomerAsync(CustomerDto customerDto);
    public Task DeleteCustomerAsync(Guid customerId);
}