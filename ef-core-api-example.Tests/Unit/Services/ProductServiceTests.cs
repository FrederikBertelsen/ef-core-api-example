using EfCoreApiExample.src.DTOs;
using EfCoreApiExample.src.Entities;
using EfCoreApiExample.src.Exceptions;
using EfCoreApiExample.src.Repositories.Interfaces;
using EfCoreApiExample.src.Services;
using NSubstitute;

namespace ef_core_api_example.Tests.Unit.Services;

public class ProductServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _productService = new ProductService(_productRepository);
    }

    #region CreateProductAsync

    [Fact]
    public async Task CreateProductAsync_WhenValidProduct_ReturnsProductDto()
    {
        // Arrange
        var createDto = new CreateProductDto("Laptop", 1000f);
        var productId = Guid.NewGuid();

        _productRepository.ProductExistsByNameAsync(createDto.Name).Returns(false);
        _productRepository.CreateProductAsync(Arg.Any<Product>())
            .Returns(callInfo =>
            {
                var p = callInfo.Arg<Product>();
                return new Product
                {
                    Id = productId,
                    Name = p.Name,
                    Price = p.Price
                };
            });

        // Act
        var result = await _productService.CreateProductAsync(createDto);

        // Assert
        Assert.Equal(productId, result.Id);
        Assert.Equal(createDto.Name, result.Name);
        Assert.Equal(createDto.Price, result.Price);

        await _productRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateProductAsync_WhenNameAlreadyExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var createDto = new CreateProductDto("Laptop", 1000f);

        _productRepository.ProductExistsByNameAsync(createDto.Name).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AlreadyExistsException>(() => _productService.CreateProductAsync(createDto));

        Assert.Contains("Product", exception.Message);
        Assert.Contains("Name", exception.Message);
        Assert.Contains(createDto.Name, exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-10f)]
    [InlineData(-0.01f)]
    public async Task CreateProductAsync_WhenPriceIsZeroOrNegative_ThrowsInvalidValueException(float price)
    {
        // Arrange
        var createDto = new CreateProductDto("Laptop", price);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidValueException>(() => _productService.CreateProductAsync(createDto));

        Assert.Contains("Price", exception.Message);
        await _productRepository.Received(0).SaveChangesAsync();
    }
    #endregion

    #region GetProductByIdAsync

    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists_ReturnsProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Mouse",
            Price = 150f
        };

        _productRepository.GetProductByIdAsync(productId).Returns(product);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        Assert.Equal(productId, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Price, result.Price);
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepository.GetProductByIdAsync(productId).Returns((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetProductByIdAsync(productId));

        Assert.Contains("Product", exception.Message);
        Assert.Contains(" Id", exception.Message);
        Assert.Contains(productId.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenIdIsEmpty_ThrowsMissingValueException()
    {
        // Arrange
        var productId = Guid.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<MissingValueException>(() => _productService.GetProductByIdAsync(productId));
    }

    #endregion

    #region GetProductByNameAsync
    [Fact]
    public async Task GetProductByNameAsync_WhenProductExists_ReturnsProductDto()
    {
        // Arrange
        var productName = "Keyboard";
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productName,
            Price = 250f
        };

        _productRepository.GetProductByNameAsync(productName).Returns(product);

        // Act
        var result = await _productService.GetProductByNameAsync(productName);

        // Assert
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(productName, result.Name);
        Assert.Equal(product.Price, result.Price);
    }

    [Fact]
    public async Task GetProductByNameAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var productName = "NonExistentProduct";

        _productRepository.GetProductByNameAsync(productName).Returns((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetProductByNameAsync(productName));

        Assert.Contains("Product", exception.Message);
        Assert.Contains("Name", exception.Message);
        Assert.Contains(productName, exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n")]
    public async Task GetProductByNameAsync_WhenNameIsEmptyOrWhitespace_ThrowsMissingValueException(string productName)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _productService.GetProductByNameAsync(productName));

        Assert.Contains("Name", exception.Message);
    }
    #endregion

    #region UpdateProductAsync
    [Fact]
    public async Task UpdateProductAsync_WhenValidUpdate_ReturnsUpdatedProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Name = "Old Name",
            Price = 50.00f
        };

        var updateDto = new ProductDto(productId, "New Name", 75.00f);

        _productRepository.GetProductByIdAsync(productId).Returns(existingProduct);
        _productRepository.UpdateProduct(Arg.Any<Product>()).Returns(callInfo => callInfo.Arg<Product>());

        // Act
        var result = await _productService.UpdateProductAsync(updateDto);

        // Assert
        Assert.Equal(updateDto.Id, result.Id);
        Assert.Equal(updateDto.Name, result.Name);
        Assert.Equal(updateDto.Price, result.Price);

        _productRepository.Received(1).UpdateProduct(existingProduct);
        await _productRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateProductAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new ProductDto(productId, "New Name", 75.00f);

        _productRepository.GetProductByIdAsync(productId).Returns((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _productService.UpdateProductAsync(updateDto));

        Assert.Contains("Product", exception.Message);
        Assert.Contains(" Id", exception.Message);
        Assert.Contains(productId.ToString(), exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateProductAsync_WhenIdIsEmpty_ThrowsMissingValueException()
    {
        // Arrange
        var productId = Guid.Empty;
        var updateDto = new ProductDto(productId, "New Name", 75.00f);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _productService.UpdateProductAsync(updateDto));

        Assert.Contains("Id", exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-10f)]
    [InlineData(-0.01f)]
    public async Task UpdateProductAsync_WhenPriceIsZeroOrNegative_ThrowsInvalidValueException(float price)
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new ProductDto(productId, "Product", price);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidValueException>(() => _productService.UpdateProductAsync(updateDto));

        Assert.Contains("Price", exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateProductAsync_WhenNameIsEmpty_ThrowsInvalidValueException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new ProductDto(productId, "", 50f);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidValueException>(() => _productService.UpdateProductAsync(updateDto));

        Assert.Contains("Name", exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }
    #endregion

    #region DeleteProductAsync
    [Fact]
    public async Task DeleteProductAsync_WhenProductExists_DeletesProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Monitor",
            Price = 300f
        };

        _productRepository.GetProductByIdAsync(productId).Returns(product);

        // Act
        await _productService.DeleteProductAsync(productId);

        // Assert
        _productRepository.Received(1).DeleteProduct(product);
        await _productRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteProductAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepository.GetProductByIdAsync(productId).Returns((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _productService.DeleteProductAsync(productId));

        Assert.Contains("Id", exception.Message);
        Assert.Contains(productId.ToString(), exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteProductAsync_WhenIdIsEmpty_ThrowsMissingValueException()
    {
        // Arrange
        var productId = Guid.Empty;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<MissingValueException>(() => _productService.DeleteProductAsync(productId));

        Assert.Contains("Id", exception.Message);

        await _productRepository.Received(0).SaveChangesAsync();
    }

    #endregion
}