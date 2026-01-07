using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Exceptions;
using EfCoreApiExample.src.Extensions;
using EfCoreApiExample.src.Repositories.Interfaces;
using EfCoreApiExample.src.Services.Interfaces;

namespace EfCoreApiExample.src.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        createCustomerDto.ValidateOrThrow();

        if (await customerRepository.CustomerExistsByEmailAsync(createCustomerDto.Email))
            throw new AlreadyExistsException("Customer", "Email", createCustomerDto.Email);

        var newCustomer = createCustomerDto.ToEntity();

        var entity = await customerRepository.CreateCustomerAsync(newCustomer);
        await customerRepository.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new MissingValueException("Customer Id");

        var customer = await customerRepository.GetCustomerByIdAsync(customerId);
        if (customer is null)
            throw new NotFoundException("Customer", "Id", customerId);

        return customer.ToDto();
    }

    public async Task<CustomerDto> UpdateCustomerAsync(CustomerDto customerDto)
    {
        if (customerDto is null)
            throw new MissingValueException("Customer data");
        if (customerDto.Id == Guid.Empty)
            throw new MissingValueException("Customer", nameof(customerDto.Id));

        var customer = await customerRepository.GetCustomerByIdAsync(customerDto.Id);
        if (customer is null)
            throw new NotFoundException("Customer", "Id", customerDto.Id);

        // update Entity field only if DTO field is not null
        customer.ApplyNonNullValues(customerDto);

        var updatedCustomer = customerRepository.UpdateCustomer(customer);
        await customerRepository.SaveChangesAsync();

        return updatedCustomer.ToDto();
    }

    public async Task DeleteCustomerAsync(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new MissingValueException("Customer Id");

        var customer = await customerRepository.GetCustomerByIdAsync(customerId);
        if (customer is null)
            throw new NotFoundException("Customer", "Id", customerId);

        customerRepository.DeleteCustomer(customer);
        await customerRepository.SaveChangesAsync();
    }
}