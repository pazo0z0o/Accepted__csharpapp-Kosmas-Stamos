using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Queries.GetAllCategories
{
    public sealed record GetAllCategoriesQuery : IQuery<IReadOnlyCollection<Category>>;
}
