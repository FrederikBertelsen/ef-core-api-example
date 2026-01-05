using EfCoreApiTemplate.src.DTOs;
using EfCoreApiTemplate.src.Extensions;
using EfCoreApiTemplate.src.Repositories.Interfaces;
using EfCoreApiTemplate.src.Services.Interfaces;

namespace EfCoreApiTemplate.src.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        createCustomerDto.ValidateOrThrow();

        if (await customerRepository.CustomerExistsByEmailAsync(createCustomerDto.Email))
            throw new ArgumentException($"A Customer with Email '{createCustomerDto.Email}' already exists");

        var newCustomer = createCustomerDto.ToEntity();

        var entity = await customerRepository.CreateCustomerAsync(newCustomer);
        await customerRepository.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException($"The Customer ID cannot be empty");

        var customer = await customerRepository.GetCustomerByIdAsync(customerId);
        if (customer is null)
            throw new ArgumentException($"No customer found with ID '{customerId}'");

        return customer.ToDto();
    }

    public async Task<CustomerDto> UpdateCustomerAsync(CustomerDto customerDto)
    {
        ArgumentNullException.ThrowIfNull(customerDto);
        if (customerDto.Id == Guid.Empty)
            throw new ArgumentException("The Customer is missing an ID");

        var customer = await customerRepository.GetCustomerByIdAsync(customerDto.Id);
        if (customer is null)
            throw new ArgumentException($"No customer found with ID '{customerDto.Id}'");

        // update Entity field only if DTO field is not null
        customer.ApplyNonNullValues(customerDto);

        var updatedCustomer = customerRepository.UpdateCustomer(customer);
        await customerRepository.SaveChangesAsync();

        return updatedCustomer.ToDto();
    }

    public async Task DeleteCustomerAsync(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException($"The Customer ID cannot be empty");

        var customer = await customerRepository.GetCustomerByIdAsync(customerId);
        if (customer is null)
            throw new ArgumentException($"No customer found with ID '{customerId}'");

        customerRepository.DeleteCustomer(customer);
        await customerRepository.SaveChangesAsync();
    }
}