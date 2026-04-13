using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Category image is required")
                .Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                .WithMessage("Category image must be a valid URL");
        }
    }
}
