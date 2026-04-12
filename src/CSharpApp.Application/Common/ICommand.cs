using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Common
{
    public interface ICommand : IRequest
    {
    }

   
    // Base interface for commands that return a value
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
