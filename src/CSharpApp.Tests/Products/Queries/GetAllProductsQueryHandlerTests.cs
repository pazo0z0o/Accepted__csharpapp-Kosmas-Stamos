using CSharpApp.Application.Products.Queries.GetAllProducts;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests.Products.Queries;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IPlatziHttpClient> _mockHttpClient;
    private readonly Mock<IOptions<RestApiSettings>> _mockSettings;
    private readonly Mock<ILogger<GetAllProductsQueryHandler>> _mockLogger;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        // Setup mocks that will be reused across all tests
        _mockHttpClient = new Mock<IPlatziHttpClient>();
        _mockSettings = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<GetAllProductsQueryHandler>>();

        // Configure settings
        _mockSettings.Setup(x => x.Value).Returns(new RestApiSettings
        {
            Products = "products"
        });

        // Create handler with mocked dependencies
        _handler = new GetAllProductsQueryHandler(
            _mockHttpClient.Object,
            _mockSettings.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsProducts_WhenApiReturnsData()
    {
        // ARRANGE
        var expectedProducts = new List<Product>
        {
            new() { Id = 1, Title = "Product 1", Price = 100 },
            new() { Id = 2, Title = "Product 2", Price = 200 }
        };

        _mockHttpClient
            .Setup(x => x.GetAsync<List<Product>>("products", default))
            .ReturnsAsync(expectedProducts);

        var query = new GetAllProductsQuery();

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyCollection_WhenApiReturnsNull()
    {
        // ARRANGE
        _mockHttpClient
            .Setup(x => x.GetAsync<List<Product>>("products", default))
            .ReturnsAsync((List<Product>)null);

        var query = new GetAllProductsQuery();

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyCollection_WhenApiReturnsEmptyList()
    {
        // ARRANGE
        _mockHttpClient
            .Setup(x => x.GetAsync<List<Product>>("products", default))
            .ReturnsAsync(new List<Product>());

        var query = new GetAllProductsQuery();

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
