using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Queries.GetAllProducts
{
    public sealed record GetAllProductsQuery : IQuery<IReadOnlyCollection<Product>>;
}
