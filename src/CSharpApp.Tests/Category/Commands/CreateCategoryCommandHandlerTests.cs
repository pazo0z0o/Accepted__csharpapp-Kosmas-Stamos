using CSharpApp.Application.Categories.Commands.CreateCategory;
using CSharpApp.Core.Dtos.Requests;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests.Category.Commands;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<IPlatziHttpClient> _mockHttpClient;
    private readonly Mock<IOptions<RestApiSettings>> _mockSettings;
    private readonly Mock<ILogger<CreateCategoryCommandHandler>> _mockLogger;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _mockHttpClient = new Mock<IPlatziHttpClient>();
        _mockSettings = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<CreateCategoryCommandHandler>>();

        _mockSettings.Setup(x => x.Value).Returns(new RestApiSettings
        {
            Categories = "categories"
        });

        _handler = new CreateCategoryCommandHandler(
            _mockHttpClient.Object,
            _mockSettings.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCreatedCategory_WhenSuccessful()
    {
        // ARRANGE
        var command = new CreateCategoryCommand
        {
            Name = "New Category",
            Image = "https://example.com/image.jpg"
        };

        var expectedCategory = new CSharpApp.Core.Dtos.Category
        {
            Id = 10,
            Name = command.Name,
            Image = command.Image
        };

        _mockHttpClient
            .Setup(x => x.PostAsync<CreateCategoryRequest, CSharpApp.Core.Dtos.Category>(
                "categories",
                It.IsAny<CreateCategoryRequest>(),
                default))
            .ReturnsAsync(expectedCategory);

        // ACT
        var result = await _handler.Handle(command, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenCreationFails()
    {
        // ARRANGE
        var command = new CreateCategoryCommand
        {
            Name = "New Category",
            Image = "https://example.com/image.jpg"
        };

        _mockHttpClient
            .Setup(x => x.PostAsync<CreateCategoryRequest, CSharpApp.Core.Dtos.Category>(
                "categories",
                It.IsAny<CreateCategoryRequest>(),
                default))
            .ReturnsAsync((CSharpApp.Core.Dtos.Category)null);

        // ACT
        var result = await _handler.Handle(command, default);

        // ASSERT
        result.Should().BeNull();
    }
}
