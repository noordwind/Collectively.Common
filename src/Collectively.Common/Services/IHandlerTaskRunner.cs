using System;
using System.Threading.Tasks;

namespace Collectively.Common.Services
{
    public interface IHandlerTaskRunner
    {
        IHandlerTask Run(Action run);
        IHandlerTask Run(Func<Task> runAsync);         
    }
}