using System;
using System.Threading.Tasks;
using Coolector.Common.Domain;
using NLog;

namespace Coolector.Common.Services
{
    public class HandlerTask : IHandlerTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHandler _handler;
        private readonly Action _run;
        private readonly Func<Task> _runAsync;
        private Action _always;
        private Func<Task> _alwaysAsync;
        private Action _onSuccess;
        private Func<Task> _onSuccessAsync;
        private Action<Exception> _onError;
        private Action<Exception, Logger> _onErrorWithLogger;
        private Action<CoolectorException> _onCustomError;
        private Action<CoolectorException, Logger> _onCustomErrorWithLogger;
        private Func<Exception, Task> _onErrorAsync;
        private Func<Exception, Logger, Task> _onErrorWithLoggerAsync;
        private Func<CoolectorException, Task> _onCustomErrorAsync;
        private Func<CoolectorException, Logger, Task> _onCustomErrorWithLoggerAsync;
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

        public IHandlerTask OnCustomError(Action<CoolectorException, Logger> onCustomError, bool propagateException = false)
        {
            _onCustomErrorWithLogger = onCustomError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnCustomError(Func<CoolectorException, Task> onCustomError, bool propagateException = false)
        {
            _onCustomErrorAsync = onCustomError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnCustomError(Func<CoolectorException, Logger, Task> onCustomError, bool propagateException = false)
        {
            _onCustomErrorWithLoggerAsync = onCustomError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Action<Exception> onError, bool propagateException = false)
        {
            _onError = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Action<Exception, Logger> onError, bool propagateException = false)
        {
            _onErrorWithLogger = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Func<Exception, Task> onError, bool propagateException = false)
        {
            _onErrorAsync = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Func<Exception, Logger, Task> onError, bool propagateException = false)
        {
            _onErrorWithLoggerAsync = onError;
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
                    _onCustomErrorWithLogger?.Invoke(customException, Logger);
                    _onCustomError?.Invoke(customException);
                }
                _onErrorWithLogger?.Invoke(customException, Logger);
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
                if (customException != null)
                {
                    _onCustomErrorWithLogger?.Invoke(customException, Logger);
                    if (_onCustomErrorWithLoggerAsync != null)
                    {
                        await _onCustomErrorWithLoggerAsync(customException, Logger);
                    }
                    _onCustomError?.Invoke(customException);
                    if (_onCustomErrorAsync != null)
                    {
                        await _onCustomErrorAsync(customException);
                    }
                }
                _onErrorWithLogger?.Invoke(customException, Logger);
                if (_onErrorWithLoggerAsync != null)
                {
                    await _onErrorWithLoggerAsync(exception, Logger);
                }
                _onError?.Invoke(exception);
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