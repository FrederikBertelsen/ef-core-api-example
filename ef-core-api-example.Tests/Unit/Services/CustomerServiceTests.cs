using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Entities;
using EfCoreApiExample.src.Exceptions;
using EfCoreApiExample.src.Repositories.Interfaces;
using EfCoreApiExample.src.Services;
using NSubstitute;

namespace ef_core_api_example.Tests.Unit.Services;

public class CustomerServiceTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepository);
    }

    #region CreateCustomerAsync

    [Fact]
    public async Task CreateCustomerAsync_WhenValidCustomer_ReturnsCustomerDto()
    {
        // Arrange
        var createDto = new CreateCustomerDto("Lars", "Larsen", "lars@example.dk", "123 Lars Street");
        var customerId = Guid.NewGuid();

        _customerRepository.CustomerExistsByEmailAsync(createDto.Email).Returns(false);
        _customerRepository.CreateCustomerAsync(Arg.Any<Customer>())
            .Returns(callInfo =>
            {
                var c = callInfo.Arg<Customer>();
                return new Customer
                {
                    Id = customerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Address = c.Address
                };
            });

        // Act
        var result = await _customerService.CreateCustomerAsync(createDto);

        // Assert
        Assert.Equal(customerId, result.Id);
        Assert.Equal(createDto.FirstName, result.FirstName);
        Assert.Equal(createDto.LastName, result.LastName);
        Assert.Equal(createDto.Email, result.Email);
        Assert.Equal(createDto.Address, result.Address);

        await _customerRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateCustomerAsync_WhenEmailAlreadyExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var createDto = new CreateCustomerDto("Lars", "Larsen", "existing@example.com", "123 Lars Street");

        _customerRepository.CustomerExistsByEmailAsync(createDto.Email).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AlreadyExistsException>(() => _customerService.CreateCustomerAsync(createDto));

        Assert.Contains("Email", exception.Message);
        Assert.Contains(createDto.Email, exception.Message);

        await _customerRepository.Received(0).SaveChangesAsync();
    }

    #endregion

    #region GetCustomerByIdAsync

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerExists_ReturnsCustomerDto()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Ole",
            LastName = "Olesen",
            Email = "ole@example.dk",
            Address = "123 Ole Street"
        };

        _customerRepository.GetCustomerByIdAsync(customerId).Returns(customer);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(customerId);

        // Assert
        Assert.Equal(customerId, result.Id);
        Assert.Equal(customer.FirstName, result.FirstName);
        Assert.Equal(customer.LastName, result.LastName);
        Assert.Equal(customer.Email, result.Email);
        Assert.Equal(customer.Address, result.Address);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _customerRepository.GetCustomerByIdAsync(customerId).Returns((Customer?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _customerService.GetCustomerByIdAsync(customerId));

        Assert.Contains("Customer", exception.Message);
        Assert.Contains(" Id", exception.Message);
        Assert.Contains(customerId.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenIdIsEmpty_ThrowsMissingValueException()
    {
        // Arrange
        var customerId = Guid.Empty;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _customerService.GetCustomerByIdAsync(customerId));

        Assert.Contains("Id", exception.Message);
    }

    #endregion

    #region UpdateCustomerAsync

    [Fact]
    public async Task UpdateCustomerAsync_WhenValidUpdate_ReturnsUpdatedCustomerDto()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer
        {
            Id = customerId,
            FirstName = "Ole",
            LastName = "Olesen",
            Email = "ole@example.dk",
            Address = "123 Ole Street"
        };

        var updateDto = new CustomerDto(customerId, "Peter", "Petersen", "peter@example.dk", "456 Peter Street");

        _customerRepository.GetCustomerByIdAsync(customerId).Returns(existingCustomer);
        _customerRepository.UpdateCustomer(Arg.Any<Customer>()).Returns(callInfo => callInfo.Arg<Customer>());

        // Act
        var result = await _customerService.UpdateCustomerAsync(updateDto);

        // Assert
        Assert.Equal(updateDto.Id, result.Id);
        Assert.Equal(updateDto.FirstName, result.FirstName);
        Assert.Equal(updateDto.LastName, result.LastName);
        Assert.Equal(updateDto.Email, result.Email);
        Assert.Equal(updateDto.Address, result.Address);

        _customerRepository.Received(1).UpdateCustomer(existingCustomer);
        await _customerRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenCustomerNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var updateDto = new CustomerDto(customerId, "Jens", "Jensen", "jens@example.dk", "789 Jensen St");

        _customerRepository.GetCustomerByIdAsync(customerId).Returns((Customer?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _customerService.UpdateCustomerAsync(updateDto));

        Assert.Contains("Customer", exception.Message);
        Assert.Contains(" Id", exception.Message);
        Assert.Contains(customerId.ToString(), exception.Message);

        await _customerRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenIdIsEmpty_ThrowsMissingValueException()
    {
        // Arrange
        var updateDto = new CustomerDto(Guid.Empty, "Jens", "Jensen", "jens@example.dk", "789 Jensen St");

        // Act & Assert
        await Assert.ThrowsAsync<MissingValueException>(() => _customerService.UpdateCustomerAsync(updateDto));

        await _customerRepository.Received(0).SaveChangesAsync();
    }

    #endregion

    #region DeleteCustomerAsync

    [Fact]
    public async Task DeleteCustomerAsync_WhenCustomerExists_DeletesCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Niels",
            LastName = "Nielsen",
            Email = "niels@example.com",
            Address = "123 Nielsen St"
        };

        _customerRepository.GetCustomerByIdAsync(customerId).Returns(customer);

        // Act
        await _customerService.DeleteCustomerAsync(customerId);

        // Assert
        _customerRepository.Received(1).DeleteCustomer(customer);
        await _customerRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteCustomerAsync_WhenCustomerNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _customerRepository.GetCustomerByIdAsync(customerId).Returns((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _customerService.DeleteCustomerAsync(customerId));

        await _customerRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteCustomerAsync_WhenIdIsEmpty_ThrowsMissingValueException()
    {
        // Arrange
        var customerId = Guid.Empty;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _customerService.DeleteCustomerAsync(customerId));

        Assert.Contains("Id", exception.Message);

        await _customerRepository.Received(0).SaveChangesAsync();
    }

    #endregion
}
