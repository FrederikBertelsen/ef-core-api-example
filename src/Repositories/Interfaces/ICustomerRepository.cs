using EfCoreApiTemplate.src.DTOs;

interface ICustomerRepository
{
    public Task<CustomerDto> CreateCustomer(CreateCustomerDto newCustomer);
    public Task<CustomerDto> PatchCustomer(CustomerDto customer);
    public Task DeleteCustomer(Guid customerId);
}