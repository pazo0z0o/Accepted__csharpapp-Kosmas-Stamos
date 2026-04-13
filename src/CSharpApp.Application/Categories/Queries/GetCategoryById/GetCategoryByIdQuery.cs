using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Queries.GetCategoryById
{
    public sealed record GetCategoryByIdQuery(int CategoryId) : IQuery<Category?>;
}
