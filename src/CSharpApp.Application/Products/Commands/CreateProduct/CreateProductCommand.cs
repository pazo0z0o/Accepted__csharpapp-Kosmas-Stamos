using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Commands.CreateProduct
{
    public sealed record CreateProductCommand : ICommand<Product?>
    {
        public string Title { get; init; } = string.Empty;
        public int Price { get; init; }
        public string Description { get; init; } = string.Empty;
        public int CategoryId { get; init; }
        public List<string> Images { get; init; } = [];
    }
}
