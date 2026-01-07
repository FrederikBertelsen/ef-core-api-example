using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Entities;
using EfCoreApiExample.src.Exceptions;
using EfCoreApiExample.src.Repositories.Interfaces;
using EfCoreApiExample.src.Services;
using NSubstitute;

namespace ef_core_api_example.Tests.Unit.Services;

public class OrderServiceTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _orderItemRepository = Substitute.For<IOrderItemRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();

        _orderService = new OrderService(_orderRepository, _orderItemRepository, _productRepository, _customerRepository);
    }

    #region CreateOrderAsync

    [Fact]
    public async Task CreateOrderAsync_WhenValidOrder_ReturnsOrderDto()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Lars",
            LastName = "Larsen",
            Email = "larss@example.com",
            Address = "123 Larsen St"
        };

        var product1 = new Product { Id = productId1, Name = "Phone", Price = 2500f };
        var product2 = new Product { Id = productId2, Name = "Tablet", Price = 4000f };

        var createDto = new CreateOrderDto(
            customerId,
            [
                new CreateOrderItemDto(productId1, 2),
                new CreateOrderItemDto(productId2, 1)
            ]
        );

        _customerRepository.GetCustomerByIdAsync(customerId).Returns(customer);
        _productRepository.GetProductsByIdAsync(Arg.Any<List<Guid>>()).Returns([product1, product2]);
        _orderRepository.CreateOrderAsync(Arg.Any<Order>())
            .Returns(callInfo =>
            {
                var o = callInfo.Arg<Order>();
                var createdOrder = new Order
                {
                    CustomerId = o.CustomerId,
                    Customer = o.Customer,
                    OrderItems = o.OrderItems
                };
                typeof(Order).GetProperty("Id")!.SetValue(createdOrder, orderId);
                return createdOrder;
            });

        // Act
        var result = await _orderService.CreateOrderAsync(createDto);

        // Assert
        Assert.Equal(orderId, result.Id);
        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(2, result.OrderItems.Count);

        await _orderRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateOrderAsync_WhenCustomerNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var createDto = new CreateOrderDto(
            customerId,
            [new CreateOrderItemDto(productId, 1)]
        );

        _customerRepository.GetCustomerByIdAsync(customerId).Returns((Customer?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.CreateOrderAsync(createDto));

        Assert.Contains("Customer", exception.Message);
        Assert.Contains(" Id", exception.Message);
        Assert.Contains(customerId.ToString(), exception.Message);

        await _orderRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateOrderAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Ole",
            LastName = "Olesen",
            Email = "ole@example.com",
            Address = "123 Olesen St"
        };

        var product1 = new Product { Id = productId1, Name = "Phone", Price = 1500f };

        var createDto = new CreateOrderDto(
            customerId,
            [
                new CreateOrderItemDto(productId1, 2),
                new CreateOrderItemDto(productId2, 1)
            ]
        );

        _customerRepository.GetCustomerByIdAsync(customerId).Returns(customer);
        _productRepository.GetProductsByIdAsync(Arg.Any<List<Guid>>()).Returns([product1]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.CreateOrderAsync(createDto));

        Assert.Contains("Product", exception.Message);
        Assert.Contains(" Id", exception.Message);

        await _orderRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task CreateOrderAsync_WhenQuantityIsZeroOrNegative_ThrowsInvalidValueException(int quantity)
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var createDto = new CreateOrderDto(
            customerId,
            [new CreateOrderItemDto(productId, quantity)]
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidValueException>(() => _orderService.CreateOrderAsync(createDto));

        await _orderRepository.Received(0).SaveChangesAsync();
    }
    #endregion

    #region GetOrderByIdAsync
    [Fact]

    public async Task GetOrderByIdAsync_WhenOrderExists_ReturnsOrderDto()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Jens",
            LastName = "Jensen",
            Email = "jens@example.com",
            Address = "456 Jensen St"
        };

        var order = new Order
        {
            Id = orderId,
            CustomerId = customerId,
            Customer = customer,
            OrderItems = []
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act
        var result = await _orderService.GetOrderByIdAsync(orderId);

        // Assert
        Assert.Equal(orderId, result.Id);
        Assert.Equal(customerId, result.CustomerId);
    }

    [Fact]
    public async Task GetOrderByIdAsync_WhenOrderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepository.GetOrderByIdAsync(orderId).Returns((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetOrderByIdAsync(orderId));

        Assert.Contains("Order", exception.Message);
        Assert.Contains(" Id", exception.Message);
        Assert.Contains(orderId.ToString(), exception.Message);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task GetOrderByIdAsync_WhenIdIsEmpty_ThrowsMissingValueException(string guidString)
    {
        // Arrange
        var orderId = Guid.Parse(guidString);

        // Act & Assert
        await Assert.ThrowsAsync<MissingValueException>(() => _orderService.GetOrderByIdAsync(orderId));
    }

    #endregion

    #region AddProductsToOrderAsync

    [Fact]
    public async Task AddProductsToOrderAsync_WhenValidProducts_AddsProductsToOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var product = new Product { Id = productId, Name = "Oven", Price = 2400f };

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = []
        };

        var productsToAdd = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Fridge", 5200f), 1)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);
        _productRepository.GetProductByIdAsync(productId).Returns(product);

        // Act
        await _orderService.AddProductsToOrderAsync(orderId, productsToAdd);

        // Assert
        await _orderItemRepository.Received(1).CreateOrderItemsAsync(Arg.Any<List<OrderItem>>());
        await _orderItemRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task AddProductsToOrderAsync_WhenOrderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var productsToAdd = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Electric Toothbrush", 450f), 2)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.AddProductsToOrderAsync(orderId, productsToAdd));

        Assert.Contains("Order", exception.Message);
        Assert.Contains(" Id", exception.Message);

        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task AddProductsToOrderAsync_WhenOrderIsCompleted_ThrowsBusinessLogicException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = []
        };
        order.MarkAsCompleted();

        var productsToAdd = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Santa Hat", 80f), 10)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(() => _orderService.AddProductsToOrderAsync(orderId, productsToAdd));

        Assert.Contains("completed", exception.Message);

        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task AddProductsToOrderAsync_WhenIdIsEmpty_ThrowsMissingValueException(string guidString)
    {
        // Arrange
        var orderId = Guid.Parse(guidString);
        var productsToAdd = new List<OrderItemDto>
        {
            new(new ProductDto(Guid.NewGuid(), "Laptop", 4000f), 2)
        };

        // Act & Assert
        await Assert.ThrowsAsync<MissingValueException>(() => _orderService.AddProductsToOrderAsync(orderId, productsToAdd));

        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    #endregion

    #region RemoveProductsFromOrderAsync

    [Fact]
    public async Task RemoveProductsFromOrderAsync_WhenValidProducts_RemovesProductsFromOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var product = new Product { Id = productId, Name = "Oven", Price = 5500f };

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            Product = product,
            Quantity = 5
        };

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = [orderItem]
        };

        var productsToRemove = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Oven", 5500f), 2)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act
        await _orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove);

        // Assert
        Assert.Equal(3, orderItem.Quantity);
        await _orderItemRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveProductsFromOrderAsync_WhenExactQuantityRemoved_RemovesOrderItem()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var product = new Product { Id = productId, Name = "Fridge", Price = 5200f };

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            Product = product,
            Quantity = 2
        };

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = [orderItem]
        };

        var productsToRemove = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Fridge", 5200f), 2)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act
        await _orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove);

        // Assert
        _orderItemRepository.Received(1).DeleteOrderItems(Arg.Is<List<OrderItem>>(list => list.Contains(orderItem)));
        await _orderItemRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveProductsFromOrderAsync_WhenQuantityExceeded_ThrowsBusinessLogicException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var product = new Product { Id = productId, Name = "Fridge", Price = 5200f };

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            Product = product,
            Quantity = 2
        };

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = [orderItem]
        };

        var productsToRemove = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Fridge", 5200f), 5)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(() => _orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove));

        Assert.Contains(productId.ToString(), exception.Message);

        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveProductsFromOrderAsync_WhenOrderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var productsToRemove = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Phone", 10000f), 2)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove));

        Assert.Contains("Order", exception.Message);

        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveProductsFromOrderAsync_WhenOrderIsCompleted_ThrowsBusinessLogicException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = []
        };
        order.MarkAsCompleted();

        var productsToRemove = new List<OrderItemDto>
        {
            new(new ProductDto(productId, "Tablet", 9000f), 4)
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(() => _orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove));

        Assert.Contains("completed", exception.Message);

        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task RemoveProductsFromOrderAsync_WhenIdIsEmpty_ThrowsMissingValueException(string guidString)
    {
        // Arrange
        var orderId = Guid.Parse(guidString);
        var productsToRemove = new List<OrderItemDto>
        {
            new(new ProductDto(Guid.NewGuid(), "Phone", 3500f), 20)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _orderService.RemoveProductsFromOrderAsync(orderId, productsToRemove));

        Assert.Contains("Order", exception.Message);
        Assert.Contains(" Id", exception.Message);
        await _orderItemRepository.Received(0).SaveChangesAsync();
    }

    #endregion

    #region MarkOrderAsCompletedAsync
    [Fact]
    public async Task MarkOrderAsCompletedAsync_WhenOrderExists_MarksOrderAsCompleted()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = []
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act
        await _orderService.MarkOrderAsCompletedAsync(orderId);

        // Assert
        Assert.True(order.IsCompleted);
        await _orderRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task MarkOrderAsCompletedAsync_WhenOrderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepository.GetOrderByIdAsync(orderId).Returns((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.MarkOrderAsCompletedAsync(orderId));

        Assert.Contains("Order", exception.Message);
        Assert.Contains(" Id", exception.Message);

        await _orderRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task MarkOrderAsCompletedAsync_WhenOrderAlreadyCompleted_ThrowsBusinessLogicException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = []
        };
        order.MarkAsCompleted();

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(() => _orderService.MarkOrderAsCompletedAsync(orderId));

        Assert.Contains("already completed", exception.Message);

        await _orderRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task MarkOrderAsCompletedAsync_WhenIdIsEmpty_ThrowsMissingValueException(string guidString)
    {
        // Arrange
        var orderId = Guid.Parse(guidString);

        // Act & Assert
        await Assert.ThrowsAsync<MissingValueException>(() => _orderService.MarkOrderAsCompletedAsync(orderId));

        await _orderRepository.Received(0).SaveChangesAsync();
    }
    #endregion

    #region DeleteOrderAsync
    [Fact]
    public async Task DeleteOrderAsync_WhenOrderExists_DeletesOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderItems = []
        };

        _orderRepository.GetOrderByIdAsync(orderId).Returns(order);

        // Act
        await _orderService.DeleteOrderAsync(orderId);

        // Assert
        _orderRepository.Received(1).DeleteOrder(order);
        await _orderRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteOrderAsync_WhenOrderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepository.GetOrderByIdAsync(orderId).Returns((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.DeleteOrderAsync(orderId));

        Assert.Contains("Order", exception.Message);
        Assert.Contains(" Id", exception.Message);

        await _orderRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task DeleteOrderAsync_WhenIdIsEmpty_ThrowsMissingValueException(string guidString)
    {
        // Arrange
        var orderId = Guid.Parse(guidString);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _orderService.DeleteOrderAsync(orderId));

        Assert.Contains("Order", exception.Message);
        Assert.Contains(" Id", exception.Message);

        await _orderRepository.Received(0).SaveChangesAsync();
    }

    #endregion
}