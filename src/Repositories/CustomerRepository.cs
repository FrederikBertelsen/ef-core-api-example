using EfCoreApiExample.src.Data;
using EfCoreApiExample.src.Entities;
using EfCoreApiExample.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiExample.src.Repositories;

public class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    public async Task<Customer> CreateCustomerAsync(Customer customer) => (await dbContext.Customers.AddAsync(customer)).Entity;
    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId) => 
        await dbContext.Customers
        .Include(customer => customer.Orders)
        .FirstOrDefaultAsync(customer => customer.Id == customerId);
    public async Task<Customer?> GetCustomerByEmailAsync(string email) => 
        await dbContext.Customers
        .Include(customer => customer.Orders)
        .FirstOrDefaultAsync(c => c.Email == email);
    public Customer UpdateCustomer(Customer customer) => dbContext.Customers.Update(customer).Entity;
    public void DeleteCustomer(Customer customer) => dbContext.Customers.Remove(customer);
    public Task<bool> CustomerExistsByIdAsync(Guid customerId) => dbContext.Customers.AnyAsync(c => c.Id == customerId);
    public Task<bool> CustomerExistsByEmailAsync(string email) => dbContext.Customers.AnyAsync(c => c.Email == email);
    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}