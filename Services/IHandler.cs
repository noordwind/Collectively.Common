using System;
using System.Threading.Tasks;

namespace Coolector.Common.Services
{
    public interface IHandler
    {
        IHandlerTask Run(Action action);
        IHandlerTask Run(Func<Task> actionAsync);
        void Execute();
        Task ExecuteAsync();
    }
}