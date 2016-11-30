using System;
using System.Threading.Tasks;
using Coolector.Common.Domain;

namespace Coolector.Common.Services
{
    public interface IHandlerTask
    {
        IHandlerTask Always(Action always);
        IHandlerTask Always(Func<Task> always);
        IHandlerTask OnCustomError(Action<CoolectorException> onCustomError, bool propagateException = false);
        IHandlerTask OnCustomError(Func<CoolectorException, Task> onCustomError, bool propagateException = false);
        IHandlerTask OnError(Action<Exception> onError, bool propagateException = false);
        IHandlerTask OnError(Func<Exception, Task> onError, bool propagateException = false);
        IHandlerTask OnSuccess(Action onSuccess);
        IHandlerTask OnSuccess(Func<Task> onSuccess);
        IHandlerTask PropagateException();
        IHandlerTask DoNotPropagateException();
        IHandler Next();
        void Execute();
        Task ExecuteAsync();
    }
}