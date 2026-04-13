using CSharpApp.Application.Categories.Queries.GetAllCategories;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests.Category.Queries;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<IPlatziHttpClient> _mockHttpClient;
    private readonly Mock<IOptions<RestApiSettings>> _mockSettings;
    private readonly Mock<ILogger<GetAllCategoriesQueryHandler>> _mockLogger;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _mockHttpClient = new Mock<IPlatziHttpClient>();
        _mockSettings = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<GetAllCategoriesQueryHandler>>();

        _mockSettings.Setup(x => x.Value).Returns(new RestApiSettings
        {
            Categories = "categories"
        });

        _handler = new GetAllCategoriesQueryHandler(
            _mockHttpClient.Object,
            _mockSettings.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCategories_WhenApiReturnsData()
    {
        // ARRANGE
        var expectedCategories = new List<CSharpApp.Core.Dtos.Category>
        {
            new() { Id = 1, Name = "Category 1" },
            new() { Id = 2, Name = "Category 2" }
        };

        _mockHttpClient
            .Setup(x => x.GetAsync<List<CSharpApp.Core.Dtos.Category>>("categories", default))
            .ReturnsAsync(expectedCategories);

        var query = new GetAllCategoriesQuery();

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyCollection_WhenApiReturnsNull()
    {
        // ARRANGE
        _mockHttpClient
            .Setup(x => x.GetAsync<List<CSharpApp.Core.Dtos.Category>>("categories", default))
            .ReturnsAsync((List<CSharpApp.Core.Dtos.Category>)null);

        var query = new GetAllCategoriesQuery();

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
