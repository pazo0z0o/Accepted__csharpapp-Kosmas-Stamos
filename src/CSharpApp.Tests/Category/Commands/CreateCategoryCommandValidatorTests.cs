using CSharpApp.Application.Categories.Commands.CreateCategory;
using FluentAssertions;

namespace CSharpApp.Tests.Category.Commands;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryCommandValidatorTests()
    {
        _validator = new CreateCategoryCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // ARRANGE
        var command = new CreateCategoryCommand
        {
            Name = "Valid Category",
            Image = "https://example.com/image.jpg"
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyName_FailsValidation()
    {
        // ARRANGE
        var command = new CreateCategoryCommand
        {
            Name = "",
            Image = "https://example.com/image.jpg"
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Name));
    }

    [Fact]
    public void Validate_InvalidImageUrl_FailsValidation()
    {
        // ARRANGE
        var command = new CreateCategoryCommand
        {
            Name = "Valid Category",
            Image = "not-a-valid-url"
        };

        // ACT
        var result = _validator.Validate(command);

        // ASSERT
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Image));
    }
}
