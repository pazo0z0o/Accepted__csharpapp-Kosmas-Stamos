using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Products.Commands.CreateProduct;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Dtos.Requests;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests.Products.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IPlatziHttpClient> _mockHttpClient;
    private readonly Mock<IOptions<RestApiSettings>> _mockSettings;
    private readonly Mock<ILogger<CreateProductCommandHandler>> _mockLogger;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _mockHttpClient = new Mock<IPlatziHttpClient>();
        _mockSettings = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<CreateProductCommandHandler>>();

        _mockSettings.Setup(x => x.Value).Returns(new RestApiSettings
        {
            Products = "products"
        });

        _handler = new CreateProductCommandHandler(
            _mockHttpClient.Object,
            _mockSettings.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCreatedProduct_WhenSuccessful()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "New Product",
            Price = 150,
            Description = "New Description",
            CategoryId = 1,
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        var expectedProduct = new Product
        {
            Id = 100,
            Title = command.Title,
            Price = command.Price,
            Description = command.Description
        };

        _mockHttpClient
            .Setup(x => x.PostAsync<CreateProductRequest, Product>(
                "products",
                It.IsAny<CreateProductRequest>(),
                default))
            .ReturnsAsync(expectedProduct);

        // ACT
        var result = await _handler.Handle(command, default);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(100);
        result.Title.Should().Be(command.Title);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenCreationFails()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "New Product",
            Price = 150,
            Description = "New Description",
            CategoryId = 1,
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        _mockHttpClient
            .Setup(x => x.PostAsync<CreateProductRequest, Product>(
                "products",
                It.IsAny<CreateProductRequest>(),
                default))
            .ReturnsAsync((Product)null);

        // ACT
        var result = await _handler.Handle(command, default);

        // ASSERT
        result.Should().BeNull();
    }
}
