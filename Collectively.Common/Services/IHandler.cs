using System;
using System.Threading.Tasks;

namespace Collectively.Common.Services
{
    public interface IHandler
    {
        IHandlerTask Run(Action run);
        IHandlerTask Run(Func<Task> runAsync);
        IHandlerTaskRunner Validate(Action validate);
        IHandlerTaskRunner Validate(Func<Task> validateAsync);
        void ExecuteAll();
        Task ExecuteAllAsync();
    }
}