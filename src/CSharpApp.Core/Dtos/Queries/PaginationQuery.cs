using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos.Queries
{
    public sealed class PaginationQuery
    {
      
        // Number of items to skip (default: 0)
        public int Offset { get; set; } = 0;

        // Maximum number of items to return
        public int Limit { get; set; } = 10;
    }
}
