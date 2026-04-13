using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Commands.CreateCategory
{
    public sealed record CreateCategoryCommand : ICommand<Category?>
    {
        public string Name { get; init; } = string.Empty;
        public string Image { get; init; } = string.Empty;
    } 
}
