using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductManagementApp.Products;
using Xunit;

namespace ProductManagementAppTests.IntegrationTests;

public class ProductTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IntegrationTestWebAppFactory _factory;
    private readonly HttpClient _client;

    public ProductTests(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Theory]
    [InlineData("pen", 1, 0)]
    [InlineData("pen", 1, 1)]
    public async Task Product_is_created_successfully_with_valid_properties(string productName, decimal price,
        int stockQuantity)
    {
        //Arrange
        var command = new CreateProductCommand
        {
            ProductName = productName,
            Price = price,
            StockQuanity = stockQuantity
        };

        //Act
        var response = await _client.PostAsJsonAsync("api/Products", command);

        //Assert
        response.Should().BeSuccessful();
        var productId = await response.Content.ReadFromJsonAsync<Guid>();
        productId.Should().NotBeEmpty();
        
    }

    [Fact]
    public async Task Product_is_removed_successfully()
    {
        //Arrange
        var creationResponse = await _client.PostAsJsonAsync("api/Products",  new CreateProductCommand
        {
            ProductName = "Pen",
            Price = 100,
            StockQuanity = 0
        });
        var productId = await creationResponse.Content.ReadFromJsonAsync<Guid>();
        
        
        //Act
        var response = await _client.DeleteAsync($"api/Products/{productId}");
        
        //Assert
        response.Should().BeSuccessful();
        var getResponse = await _client.GetAsync($"api/Products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var temp = await _client.GetAsync($"api/Products/{productId}");
    }

    [Fact]
    public async Task Product_is_updated_successfully()
    {
        //Arrange
        var creationResponse = await _client.PostAsJsonAsync("api/Products",  new CreateProductCommand
        {
            ProductName = "Pen",
            Price = 100,
            StockQuanity = 0
        });
        var productId = await creationResponse.Content.ReadFromJsonAsync<Guid>();
        
        //Act
        var response = await _client.PutAsJsonAsync($"api/Products/{productId}", new CreateProductCommand
        {
            ProductName = "Pen New",
            Price = 200,
            StockQuanity = 100
        });
        
        //Assert
        response.Should().BeSuccessful();
    }
}