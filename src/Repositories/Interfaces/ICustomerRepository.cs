using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface ICustomerRepository
{
    public Task<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto);
    public Task<CustomerDto> GetCustomerById(Guid customerId);
    public Task<CustomerDto> PatchCustomer(CustomerDto customerDto);
    public Task DeleteCustomer(Guid customerId);
}