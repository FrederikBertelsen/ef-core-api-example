using System.Data;
using EfCoreApiTemplate.src.Data;
using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Mapping;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApiTemplate.src.Repositories;

public class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    public async Task<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto)
    {
        createCustomerDto.ValidateOrThrow();

        if (await dbContext.Customers.AnyAsync(customer => customer.Email == createCustomerDto.Email))
            throw new ArgumentException($"A Customer with Email '{createCustomerDto.Email}' already exists");

        var entity = (await dbContext.Customers.AddAsync(createCustomerDto.ToEntity())).Entity;
        await dbContext.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<CustomerDto> PatchCustomer(CustomerDto customerDto)
    {
        ArgumentNullException.ThrowIfNull(customerDto);
        if (customerDto.Id == Guid.Empty)
            throw new ArgumentException($"The Customer is missing an ID");

        var customer = await dbContext.Customers.FindAsync(customerDto.Id);

        if (customer is null)
            throw new ArgumentException($"No Customer found with ID '{customerDto.Id}'");

        bool updated = false;

        if (!string.IsNullOrWhiteSpace(customerDto.FirstName))
        {
            customer.FirstName = customerDto.FirstName;
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(customerDto.LastName))
        {
            customer.LastName = customerDto.LastName;
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(customerDto.Email))
        {
            customer.Email = customerDto.Email;
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(customerDto.Address))
        {
            customer.Address = customerDto.Address;
            updated = true;
        }

        if (!updated)
            throw new ArgumentException($"No new values provided to patch the customer");

        await dbContext.SaveChangesAsync();

        return customer.ToDto();
    }

    public async Task DeleteCustomer(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException($"The Customer ID cannot be empty");

        var deleted = await dbContext.Customers
            .Where(c => c.Id == customerId)
            .ExecuteDeleteAsync();

        if (deleted == 0)
            throw new ArgumentException($"No customer found with ID '{customerId}'");

        await dbContext.SaveChangesAsync();
    }
}