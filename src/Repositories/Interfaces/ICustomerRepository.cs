using EfCoreApiTemplate.src.DTOs;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface ICustomerRepository
{
    public Task<CustomerDto> CreateCustomer(CreateCustomerDto newCustomer);
    public Task<CustomerDto> PatchCustomer(CustomerDto customer);
    public Task DeleteCustomer(Guid customerId);
}