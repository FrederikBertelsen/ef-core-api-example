using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface ICustomerRepository
{
    public Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    public Task<CustomerDto> GetCustomerByIdAsync(Guid customerId);
    public Task<CustomerDto> PatchCustomerAsync(CustomerDto customerDto);
    public Task DeleteCustomerAsync(Guid customerId);
}