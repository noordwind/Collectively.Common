using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coolector.Common.Services
{
    public class Handler : IHandler
    {
        private readonly ISet<IHandlerTask> _handlerTasks = new HashSet<IHandlerTask>();

        public IHandlerTask Run(Action action)
        {
            var handlerTask = new HandlerTask(this, action);
            _handlerTasks.Add(handlerTask);

            return handlerTask;
        }

        public IHandlerTask Run(Func<Task> actionAsync)
        {
            var handlerTask = new HandlerTask(this, actionAsync);
            _handlerTasks.Add(handlerTask);

            return handlerTask;
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