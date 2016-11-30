using System;
using System.Threading.Tasks;
using Coolector.Common.Domain;

namespace Coolector.Common.Services
{
    public class HandlerTask : IHandlerTask
    {
        private readonly IHandler _handler;
        private readonly Action _run;
        private readonly Func<Task> _runAsync;
        private Action _always;
        private Func<Task> _alwaysAsync;
        private Action _onSuccess;
        private Func<Task> _onSuccessAsync;
        private Action<Exception> _onError;
        private Action<CoolectorException> _onCustomError;
        private Func<Exception, Task> _onErrorAsync;
        private Func<CoolectorException, Task> _onCustomErrorAsync;
        private bool _propagateException = true;

        public HandlerTask(IHandler handler, Action run)
        {
            _handler = handler;
            _run = run;
        }

        public HandlerTask(IHandler handler, Func<Task> runAsync)
        {
            _handler = handler;
            _runAsync = runAsync;
        }

        public IHandlerTask Always(Action always)
        {
            _always = always;

            return this;
        }

        public IHandlerTask Always(Func<Task> always)
        {
            _alwaysAsync = always;

            return this;
        }

        public IHandlerTask OnCustomError(Action<CoolectorException> onCustomError, bool propagateException = false)
        {
            _onCustomError = onCustomError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnCustomError(Func<CoolectorException, Task> onCustomError, bool propagateException = false)
        {
            _onCustomErrorAsync = onCustomError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Action<Exception> onError, bool propagateException = false)
        {
            _onError = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Func<Exception, Task> onError, bool propagateException = false)
        {
            _onErrorAsync = onError;
            _propagateException = propagateException;

            return this;
        }


        public IHandlerTask OnSuccess(Action onSuccess)
        {
            _onSuccess = onSuccess;

            return this;
        }

        public IHandlerTask OnSuccess(Func<Task> onSuccess)
        {
            _onSuccessAsync = onSuccess;

            return this;
        }

        public IHandlerTask PropagateException()
        {
            _propagateException = true;

            return this;
        }

        public IHandlerTask DoNotPropagateException()
        {
            _propagateException = false;

            return this;
        }

        public IHandler Next() => _handler;

        public void Execute()
        {
            try
            {
                _run();
                _onSuccess?.Invoke();
            }
            catch (Exception exception)
            {
                var customException = exception as CoolectorException;
                if (customException != null)
                {
                    _onCustomError?.Invoke(customException);
                }
                _onError?.Invoke(exception);
                if(_propagateException)
                {
                    throw;
                }
            }
            finally
            {
                _always?.Invoke();
            }
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await _runAsync();
                if(_onSuccessAsync != null)
                {
                    await _onSuccessAsync();
                }
            }
            catch (Exception exception)
            {
                var customException = exception as CoolectorException;
                if (_onCustomErrorAsync != null && customException != null)
                {
                    await _onCustomErrorAsync(customException);
                }
                if (_onErrorAsync != null)
                {
                    await _onErrorAsync(exception);
                }
                if(_propagateException)
                {
                    throw;
                }
            }
            finally
            {
                if (_alwaysAsync != null)
                {
                    await _alwaysAsync();
                }
            }
        }
    }
}