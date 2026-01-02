using EfCoreApiTemplate.src.DTOs;

interface ICustomerRepository
{
    public Task<CustomerDto> CreateCustomer(CreateCustomerDto NewCustomer);
    public Task<CustomerDto> PatchCustomer(CustomerDto Customer);
    public Task DeleteCustomer(Guid CustomerId);
}