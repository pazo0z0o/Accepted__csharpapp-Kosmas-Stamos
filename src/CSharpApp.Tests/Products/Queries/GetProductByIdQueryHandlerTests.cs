using CSharpApp.Application.Products.Queries.GetProductById;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IPlatziHttpClient> _mockHttpClient;
    private readonly Mock<IOptions<RestApiSettings>> _mockSettings;
    private readonly Mock<ILogger<GetProductByIdQueryHandler>> _mockLogger;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _mockHttpClient = new Mock<IPlatziHttpClient>();
        _mockSettings = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<GetProductByIdQueryHandler>>();

        _mockSettings.Setup(x => x.Value).Returns(new RestApiSettings
        {
            Products = "products"
        });

        _handler = new GetProductByIdQueryHandler(
            _mockHttpClient.Object,
            _mockSettings.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsProduct_WhenProductExists()
    {
        // ARRANGE
        var productId = 1;
        var expectedProduct = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99,
            Description = "Test Description"
        };

        _mockHttpClient
            .Setup(x => x.GetAsync<Product>($"products/{productId}", default))
            .ReturnsAsync(expectedProduct);

        var query = new GetProductByIdQuery(productId);

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Title.Should().Be("Test Product");
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenProductNotFound()
    {
        // ARRANGE
        var productId = 999;

        _mockHttpClient
            .Setup(x => x.GetAsync<Product>($"products/{productId}", default))
            .ReturnsAsync((Product)null);

        var query = new GetProductByIdQuery(productId);

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().BeNull();
    }
}
