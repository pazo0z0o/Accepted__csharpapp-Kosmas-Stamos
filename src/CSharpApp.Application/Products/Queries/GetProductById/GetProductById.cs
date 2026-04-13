using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(int ProductId) : IQuery<Product?>;
}
