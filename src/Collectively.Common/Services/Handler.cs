using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Collectively.Common.Services
{
    public class Handler : IHandler
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ISet<IHandlerTask> _handlerTasks = new HashSet<IHandlerTask>();

        public Handler(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public IHandlerTask Run(Action run)
        {
            var handlerTask = new HandlerTask(this, run, exceptionHandler: _exceptionHandler);
            _handlerTasks.Add(handlerTask);

            return handlerTask;
        }

        public IHandlerTask Run(Func<Task> runAsync)
        {
            var handlerTask = new HandlerTask(this, runAsync, exceptionHandler: _exceptionHandler);
            _handlerTasks.Add(handlerTask);

            return handlerTask;
        }

        public IHandlerTaskRunner Validate(Action validate)
            => new HandlerTaskRunner(this, _exceptionHandler, validate, null, _handlerTasks);

        public IHandlerTaskRunner Validate(Func<Task> validateAsync)
            => new HandlerTaskRunner(this, _exceptionHandler, null, validateAsync, _handlerTasks);       

        public void ExecuteAll()
        {
            foreach (var handlerTask in _handlerTasks)
            {
                handlerTask.Execute();
            }
            _handlerTasks.Clear();
        }

        public async Task ExecuteAllAsync()
        {
            foreach (var handlerTask in _handlerTasks)
            {
                await handlerTask.ExecuteAsync();
            }
            _handlerTasks.Clear();
        }
    }
}