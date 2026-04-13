using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Categories.Queries.GetCategoryById;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests.Category.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<IPlatziHttpClient> _mockHttpClient;
    private readonly Mock<IOptions<RestApiSettings>> _mockSettings;
    private readonly Mock<ILogger<GetCategoryByIdQueryHandler>> _mockLogger;
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdQueryHandlerTests()
    {
        _mockHttpClient = new Mock<IPlatziHttpClient>();
        _mockSettings = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<GetCategoryByIdQueryHandler>>();

        _mockSettings.Setup(x => x.Value).Returns(new RestApiSettings
        {
            Categories = "categories"
        });

        _handler = new GetCategoryByIdQueryHandler(
            _mockHttpClient.Object,
            _mockSettings.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCategory_WhenCategoryExists()
    {
        // ARRANGE
        var categoryId = 1;
        var expectedCategory = new CSharpApp.Core.Dtos.Category
        {
            Id = categoryId,
            Name = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        _mockHttpClient
            .Setup(x => x.GetAsync<CSharpApp.Core.Dtos.Category>($"categories/{categoryId}", default))
            .ReturnsAsync(expectedCategory);

        var query = new GetCategoryByIdQuery(categoryId);

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(categoryId);
        result.Name.Should().Be("Test Category");
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenCategoryNotFound()
    {
        // ARRANGE
        var categoryId = 999;

        _mockHttpClient
            .Setup(x => x.GetAsync<CSharpApp.Core.Dtos.Category>($"categories/{categoryId}", default))
            .ReturnsAsync((CSharpApp.Core.Dtos.Category)null);

        var query = new GetCategoryByIdQuery(categoryId);

        // ACT
        var result = await _handler.Handle(query, default);

        // ASSERT
        result.Should().BeNull();
    }
}
