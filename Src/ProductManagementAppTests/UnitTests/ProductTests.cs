using FluentAssertions;
using ProductManagementApp.Products;
using Xunit;

namespace ProductManagementAppTests.UnitTests;

public class ProductTests
{
    [Theory]
    [InlineData("pen", 1, 0)]
    [InlineData("pen", 1, 1)]
    public void Product_is_created_successfully_with_valid_properties(string productName, decimal price,
        int stockQuantity)
    {
        //Arrange
        var productId = Guid.NewGuid();

        //Act
        var sut = new Product(productId, productName, price, stockQuantity);

        //Assert
        sut.ProductId.Should().Be(productId);
        sut.ProductName.Should().Be(productName);
        sut.Price.Should().Be(price);
        sut.StockQuantity.Should().Be(stockQuantity);
    }
    
    [Theory]
    [InlineData("", 1, 0)]
    [InlineData(null, 1, 0)]
    [InlineData(" ", 1, 0)]
    [InlineData("Pen", 0, 0)]
    [InlineData("Pen", -1, 0)]
    [InlineData("Pen", 1, -1)]
    public void Product_is_not_created_with_invalid_properties(string productName, decimal price,
        int stockQuantity)
    {
        //Arrange
        var productId = Guid.NewGuid();

        //Act
        var creation =  ()=> new Product(productId, productName, price, stockQuantity);

        //Assert
        creation.Should().Throw<Exception>("Invalid properties.");
    }
}