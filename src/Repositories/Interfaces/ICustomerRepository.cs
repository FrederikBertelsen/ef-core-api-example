using EfCoreApiTemplate.src.Entities;

namespace EfCoreApiTemplate.src.Repositories.Interfaces;

public interface ICustomerRepository
{
    public Task<Customer> CreateCustomerAsync(Customer customer);
    public Task<Customer?> GetCustomerByIdAsync(Guid customerId);
    public Task<Customer?> GetCustomerByEmailAsync(string email);
    public Customer UpdateCustomer(Customer customer);
    public void DeleteCustomer(Customer customer);
    public Task<bool> CustomerExistsByIdAsync(Guid customerId);
    public Task<bool> CustomerExistsByEmailAsync(string email);
    public Task SaveChangesAsync();
}