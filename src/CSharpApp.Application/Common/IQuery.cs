using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Common
{

    // Base interface for queries
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
