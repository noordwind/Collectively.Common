using System;
using System.Threading.Tasks;
using Collectively.Common.Domain;
using NLog;

namespace Collectively.Common.Services
{
    public interface IHandlerTask
    {
        IHandlerTask Always(Action always);
        IHandlerTask Always(Func<Task> always);
        IHandlerTask OnCustomError(Action<CollectivelyException> onCustomError, 
            bool propagateException = false, bool executeOnError = false);
        IHandlerTask OnCustomError(Action<CollectivelyException, Logger> onCustomError,
            bool propagateException = false, bool executeOnError = false);
        IHandlerTask OnCustomError(Func<CollectivelyException, Task> onCustomError,
            bool propagateException = false, bool executeOnError = false);
        IHandlerTask OnCustomError(Func<CollectivelyException, Logger, Task> onCustomError,
            bool propagateException = false, bool executeOnError = false);
        IHandlerTask OnError(Action<Exception> onError, bool propagateException = false);
        IHandlerTask OnError(Action<Exception, Logger> onError, bool propagateException = false);
        IHandlerTask OnError(Func<Exception, Task> onError, bool propagateException = false);
        IHandlerTask OnError(Func<Exception, Logger, Task> onError, bool propagateException = false);
        IHandlerTask OnSuccess(Action onSuccess);
        IHandlerTask OnSuccess(Func<Task> onSuccess);
        IHandlerTask PropagateException();
        IHandlerTask DoNotPropagateException();
        IHandler Next();
        void Execute();
        Task ExecuteAsync();
    }
}