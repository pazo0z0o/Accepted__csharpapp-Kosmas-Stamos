using CSharpApp.Application.Products.Commands.CreateProduct;
using FluentAssertions;

namespace CSharpApp.Tests.Products.Commands;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "Valid Product",
            Price = 100,
            Description = "Valid description",
            CategoryId = 1,
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyTitle_FailsValidation()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "",
            Price = 100,
            Description = "Valid description",
            CategoryId = 1,
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Title));
    }

    [Fact]
    public void Validate_PriceZeroOrNegative_FailsValidation()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "Valid Product",
            Price = 0,
            Description = "Valid description",
            CategoryId = 1,
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Price));
    }

    [Fact]
    public void Validate_EmptyImages_FailsValidation()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "Valid Product",
            Price = 100,
            Description = "Valid description",
            CategoryId = 1,
            Images = new List<string>()
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Images));
    }

    [Fact]
    public void Validate_InvalidImageUrl_FailsValidation()
    {
        // ARRANGE
        var command = new CreateProductCommand
        {
            Title = "Valid Product",
            Price = 100,
            Description = "Valid description",
            CategoryId = 1,
            Images = new List<string> { "non-url-looking-string" }
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Images));
    }
}
