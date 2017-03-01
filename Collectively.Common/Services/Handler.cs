using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Collectively.Common.Services
{
    public class Handler : IHandler
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ISet<IHandlerTask> _handlerTasks = new HashSet<IHandlerTask>();
        private Action _validate;
        private Func<Task> _validateAsync;

        public Handler(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public IHandlerTask Run(Action run)
        {
            var handlerTask = new HandlerTask(this, run, _validate, _validateAsync, _exceptionHandler);
            _handlerTasks.Add(handlerTask);

            return handlerTask;
        }

        public IHandlerTask Run(Func<Task> runAsync)
        {
            var handlerTask = new HandlerTask(this, runAsync, _validate, _validateAsync, _exceptionHandler);
            _handlerTasks.Add(handlerTask);

            return handlerTask;
        }

        public IHandler Validate(Action validate)
        {
            _validate = validate;

            return this;
        }

        public IHandler Validate(Func<Task> validateAsync)
        {
            _validateAsync = validateAsync;

            return this;
        }        

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