using System;
using System.Threading.Tasks;

namespace Coolector.Common.Services
{
    public interface IHandler
    {
        IHandlerTask Run(Action run);
        IHandlerTask Run(Func<Task> runAsync);
        IHandler Validate(Action validate);
        IHandler Validate(Func<Task> validateAsync);
        void ExecuteAll();
        Task ExecuteAllAsync();
    }
}