using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Collectively.Common.Services
{
    public class HandlerTaskRunner : IHandlerTaskRunner
    {
        private readonly IHandler _handler;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly Action _validate;
        private readonly Func<Task> _validateAsync;
        private readonly ISet<IHandlerTask> _handlerTasks;

        public HandlerTaskRunner(IHandler handler, IExceptionHandler exceptionHandler,
            Action validate, Func<Task> validateAsync, ISet<IHandlerTask> handlerTasks)
        {
            _handler = handler;
            _exceptionHandler = exceptionHandler;
            _validate = validate;
            _validateAsync = validateAsync;
            _handlerTasks = handlerTasks;
        }

        public IHandlerTask Run(Action run)
        {
            var handlerTask = new HandlerTask(_handler, run, _validate, _validateAsync, _exceptionHandler);
            _handlerTasks.Add(handlerTask);

            return handlerTask;            
        }

        public IHandlerTask Run(Func<Task> runAsync)
        {
            var handlerTask = new HandlerTask(_handler, runAsync, _validate, _validateAsync, _exceptionHandler);
            _handlerTasks.Add(handlerTask);

            return handlerTask;  
        }
    }
}