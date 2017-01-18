using System;
using System.Threading.Tasks;
using Coolector.Common.Domain;
using Coolector.Common.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Common.Tests.Specs.Services
{
    public class Handler_specs
    {
        public interface IHandlerTestTask
        {
            void Execute();
            void Success();
            void Error();
            void CustomError();
            void Always();
            Task ExecuteAsync();
            Task SuccessAsync();
            Task ErrorAsync();
            Task CustomErrorAsync();
            Task AlwaysAsync();
        }

        protected static IHandler Handler;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static Mock<IHandlerTestTask> FirstTaskMock;
        protected static Mock<IHandlerTestTask> SecondTaskMock;

        protected static void Initialize()
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            FirstTaskMock = new Mock<IHandlerTestTask>();
            SecondTaskMock = new Mock<IHandlerTestTask>();

            Handler = new Handler(ExceptionHandlerMock.Object);
        }
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .Execute();

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_and_on_success_is_defined : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .OnSuccess(() => FirstTaskMock.Object.Success())
            .OnCustomError(ex => FirstTaskMock.Object.CustomError())
            .OnError(ex => FirstTaskMock.Object.Error())
            .Always(() => FirstTaskMock.Object.Always())
            .Execute();

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Once);
        It should_not_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_not_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Never);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_but_not_executed : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .OnSuccess(() => FirstTaskMock.Object.Success())
            .OnCustomError(ex => FirstTaskMock.Object.CustomError())
            .OnError(ex => FirstTaskMock.Object.Error())
            .Always(() => FirstTaskMock.Object.Always());

        It should_not_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Never);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_not_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Never);
        It should_not_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Never);
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_and_task_throws_exception : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<Exception>();
        };

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .OnSuccess(() => FirstTaskMock.Object.Success())
            .OnCustomError(ex => FirstTaskMock.Object.CustomError())
            .OnError(ex => FirstTaskMock.Object.Error())
            .Always(() => FirstTaskMock.Object.Always())
            .Execute();

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_task_throws_exception_and_it_is_propagated : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<Exception>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(() => FirstTaskMock.Object.Execute())
                .OnSuccess(() => FirstTaskMock.Object.Success())
                .OnCustomError(ex => FirstTaskMock.Object.CustomError())
                .OnError(ex => FirstTaskMock.Object.Error())
                .Always(() => FirstTaskMock.Object.Always())
                .PropagateException()
                .Execute();
        });

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
        It should_throw_an_exception = () => Exception.ShouldBeOfExactType<Exception>();
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_and_task_throws_custom_exception : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<ServiceException>();
        };

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .OnSuccess(() => FirstTaskMock.Object.Success())
            .OnCustomError(ex => FirstTaskMock.Object.CustomError())
            .OnError(ex => FirstTaskMock.Object.Error())
            .Always(() => FirstTaskMock.Object.Always())
            .Execute();

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Once);
        It should_not_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Never);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_task_throws_custom_exception_and_execute_on_error_is_enabled : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<ServiceException>();
        };

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .OnSuccess(() => FirstTaskMock.Object.Success())
            .OnCustomError(ex => FirstTaskMock.Object.CustomError(), executeOnError: true)
            .OnError(ex => FirstTaskMock.Object.Error())
            .Always(() => FirstTaskMock.Object.Always())
            .Execute();

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Once);
        It should_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_task_throws_custom_exception_and_it_is_propagated : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<ServiceException>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(() => FirstTaskMock.Object.Execute())
                .OnSuccess(() => FirstTaskMock.Object.Success())
                .OnCustomError(ex => FirstTaskMock.Object.CustomError())
                .OnError(ex => FirstTaskMock.Object.Error())
                .Always(() => FirstTaskMock.Object.Always())
                .PropagateException()
                .Execute();
        });

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_not_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Never);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Once);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
        It should_throw_a_service_exception = () => Exception.ShouldBeOfExactType<ServiceException>();
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_task_throws_custom_exception_execute_on_error_is_enabled_and_it_is_propagated : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<ServiceException>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(() => FirstTaskMock.Object.Execute())
                .OnSuccess(() => FirstTaskMock.Object.Success())
                .OnCustomError(ex => FirstTaskMock.Object.CustomError(), executeOnError: true)
                .OnError(ex => FirstTaskMock.Object.Error())
                .Always(() => FirstTaskMock.Object.Always())
                .PropagateException()
                .Execute();
        });

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Once);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
        It should_throw_a_service_exception = () => Exception.ShouldBeOfExactType<ServiceException>();
    }

    [Subject("Handler Execute")]
    public class When_one_task_is_assigned_and_task_throws_custom_exception_but_custom_error_handler_is_not_defined : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<ServiceException>();
        };

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .OnSuccess(() => FirstTaskMock.Object.Success())
            .OnError(ex => FirstTaskMock.Object.Error())
            .Always(() => FirstTaskMock.Object.Always())
            .Execute();

        It should_call_execute_method = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_error_method = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_not_call_success_method = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_call_always_method = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);
    }

    [Subject("Handler ExecuteAll")]
    public class When_two_tasks_are_assigned : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
            .Next()
            .Run(() => SecondTaskMock.Object.Execute())
            .Next()
            .ExecuteAll();

        It should_call_execute_method_on_first_task = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_execute_method_on_second_task = () => SecondTaskMock.Verify(x => x.Execute(), Times.Once);
    }

    [Subject("Handler ExecuteAll")]
    public class When_two_tasks_are_assigned_and_first_throws_exception : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<Exception>();
        };

        Because of = () => Handler
            .Run(() => FirstTaskMock.Object.Execute())
                .OnSuccess(() => FirstTaskMock.Object.Success())
                .OnCustomError(ex => FirstTaskMock.Object.CustomError())
                .OnError(ex => FirstTaskMock.Object.Error())
                .Always(() => FirstTaskMock.Object.Always())
            .Next()
            .Run(() => SecondTaskMock.Object.Execute())
                .OnSuccess(() => SecondTaskMock.Object.Success())
                .OnCustomError(ex => SecondTaskMock.Object.CustomError())
                .OnError(ex => SecondTaskMock.Object.Error())
                .Always(() => SecondTaskMock.Object.Always())
            .Next()
            .ExecuteAll();

        It should_call_execute_method_on_first_task = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_not_call_success_method_on_first_task = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method_on_first_task = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_call_error_method_on_first_task = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_call_always_method_on_first_task = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);

        It should_call_execute_method_on_second_task = () => SecondTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_call_success_method_on_second_task = () => SecondTaskMock.Verify(x => x.Success(), Times.Once);
        It should_not_call_custom_error_method_on_second_task = () => SecondTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_not_call_error_method_on_second_task = () => SecondTaskMock.Verify(x => x.Error(), Times.Never);
        It should_call_always_method_on_second_task = () => SecondTaskMock.Verify(x => x.Always(), Times.Once);
    }

    [Subject("Handler ExecuteAll")]
    public class When_two_tasks_are_assigned_first_throws_exception_and_propagates_it : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.Execute()).Throws<Exception>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(() => FirstTaskMock.Object.Execute())
                    .OnSuccess(() => FirstTaskMock.Object.Success())
                    .OnCustomError(ex => FirstTaskMock.Object.CustomError())
                    .OnError(ex => FirstTaskMock.Object.Error())
                    .Always(() => FirstTaskMock.Object.Always())
                    .PropagateException()
                .Next()
                .Run(() => SecondTaskMock.Object.Execute())
                    .OnSuccess(() => SecondTaskMock.Object.Success())
                    .OnCustomError(ex => SecondTaskMock.Object.CustomError())
                    .OnError(ex => SecondTaskMock.Object.Error())
                    .Always(() => SecondTaskMock.Object.Always())
                .Next()
                .ExecuteAll();
        });

        It should_call_execute_method_on_first_task = () => FirstTaskMock.Verify(x => x.Execute(), Times.Once);
        It should_not_call_success_method_on_first_task = () => FirstTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method_on_first_task = () => FirstTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_call_error_method_on_first_task = () => FirstTaskMock.Verify(x => x.Error(), Times.Once);
        It should_call_always_method_on_first_task = () => FirstTaskMock.Verify(x => x.Always(), Times.Once);

        It should_not_call_execute_method_on_second_task = () => SecondTaskMock.Verify(x => x.Execute(), Times.Never);
        It should_not_call_success_method_on_second_task = () => SecondTaskMock.Verify(x => x.Success(), Times.Never);
        It should_not_call_custom_error_method_on_second_task = () => SecondTaskMock.Verify(x => x.CustomError(), Times.Never);
        It should_not_call_error_method_on_second_task = () => SecondTaskMock.Verify(x => x.Error(), Times.Never);
        It should_not_call_always_method_on_second_task = () => SecondTaskMock.Verify(x => x.Always(), Times.Never);

        It should_throw_an_exception = () => Exception.ShouldBeOfExactType<Exception>();
    }

    [Subject("Handler ExecuteAll")]
    public class When_two_tasks_are_assigned_and_there_are_two_different_execute_all_calls : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () =>
        {
            Handler
                .Run(() => FirstTaskMock.Object.Execute())
                .Next()
                .Run(() => FirstTaskMock.Object.Execute())
                .Next()
                .ExecuteAll();

            Handler
                .Run(() => SecondTaskMock.Object.Execute())
                .Next()
                .Run(() => SecondTaskMock.Object.Execute())
                .Next()
                .ExecuteAll();
        };

        It should_call_execute_method_on_first_task = () => FirstTaskMock.Verify(x => x.Execute(), Times.Exactly(2));
        It should_call_execute_method_on_second_task = () => SecondTaskMock.Verify(x => x.Execute(), Times.Exactly(2));
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_async : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .ExecuteAsync()
            .Await();

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_and_on_success_is_defined_async : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
            .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
            .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
            .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
            .ExecuteAsync()
            .Await();

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Once);
        It should_not_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_not_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Never);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_but_not_executed_async : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
            .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
            .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
            .Always(async () => await FirstTaskMock.Object.AlwaysAsync());

        It should_not_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Never);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_not_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Never);
        It should_not_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Never);
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_and_task_throws_exception_async : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<Exception>();
        };

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
            .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
            .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
            .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
            .ExecuteAsync()
            .Await();

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_task_throws_exception_and_it_is_propagated_async : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<Exception>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
                .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
                .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
                .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
                .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
                .PropagateException()
                .ExecuteAsync()
                .Await();
        });

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
        It should_throw_an_exception = () => Exception.ShouldBeOfExactType<Exception>();
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_and_task_throws_custom_exception_async : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<ServiceException>();
        };

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
            .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
            .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
            .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
            .ExecuteAsync()
            .Await();

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_not_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Never);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Once);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_task_throws_custom_exception_async_and_execute_on_error_is_enabled : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<ServiceException>();
        };

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
            .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync(), executeOnError: true)
            .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
            .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
            .ExecuteAsync()
            .Await();

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Once);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_task_throws_custom_exception_and_it_is_propagated_async : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<ServiceException>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
                .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
                .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
                .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
                .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
                .PropagateException()
                .ExecuteAsync()
                .Await();
        });

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_not_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Never);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Once);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
        It should_throw_a_service_exception = () => Exception.ShouldBeOfExactType<ServiceException>();
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_task_throws_custom_exception_execute_on_error_is_enabled_and_it_is_propagated_async : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<ServiceException>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
                .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
                .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync(), executeOnError: true)
                .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
                .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
                .PropagateException()
                .ExecuteAsync()
                .Await();
        });

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Once);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
        It should_throw_a_service_exception = () => Exception.ShouldBeOfExactType<ServiceException>();
    }

    [Subject("Handler ExecuteAsync")]
    public class When_one_task_is_assigned_and_task_throws_custom_exception_but_custom_error_handler_is_not_defined_async : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<ServiceException>();
        };

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
            .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
            .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
            .ExecuteAsync()
            .Await();

        It should_call_execute_async_method = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_error_async_method = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_not_call_success_async_method = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_call_always_async_method = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAllAsync")]
    public class When_two_tasks_are_assigned_async : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
            .Next()
            .Run(async () => await SecondTaskMock.Object.ExecuteAsync())
            .Next()
            .ExecuteAllAsync()
            .Await();

        It should_call_execute_method_async_on_first_task = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_execute_method_async_on_second_task = () => SecondTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAllAsync")]
    public class When_two_tasks_are_assigned_and_first_throws_exception_async : Handler_specs
    {
        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<Exception>();
        };

        Because of = () => Handler
            .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
                .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
                .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
                .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
                .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
            .Next()
            .Run(async () => await SecondTaskMock.Object.ExecuteAsync())
                .OnSuccess(async () => await SecondTaskMock.Object.SuccessAsync())
                .OnCustomError(async ex => await SecondTaskMock.Object.CustomErrorAsync())
                .OnError(async ex => await SecondTaskMock.Object.ErrorAsync())
                .Always(async () => await SecondTaskMock.Object.AlwaysAsync())
            .Next()
            .ExecuteAllAsync()
            .Await();

        It should_call_execute_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_not_call_success_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_call_error_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_call_always_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);

        It should_call_execute_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_call_success_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.SuccessAsync(), Times.Once);
        It should_not_call_custom_error_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_not_call_error_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.ErrorAsync(), Times.Never);
        It should_call_always_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);
    }

    [Subject("Handler ExecuteAllAsync")]
    public class When_two_tasks_are_assigned_first_throws_exception_and_propagates_it_async : Handler_specs
    {
        protected static Exception Exception;

        Establish context = () =>
        {
            Initialize();
            FirstTaskMock.Setup(x => x.ExecuteAsync()).Throws<Exception>();
        };

        Because of = () => Exception = Catch.Exception(() =>
        {
            Handler
                .Run(async () => await FirstTaskMock.Object.ExecuteAsync())
                    .OnSuccess(async () => await FirstTaskMock.Object.SuccessAsync())
                    .OnCustomError(async ex => await FirstTaskMock.Object.CustomErrorAsync())
                    .OnError(async ex => await FirstTaskMock.Object.ErrorAsync())
                    .Always(async () => await FirstTaskMock.Object.AlwaysAsync())
                    .PropagateException()
                .Next()
                .Run(async () => await SecondTaskMock.Object.ExecuteAsync())
                    .OnSuccess(async () => await SecondTaskMock.Object.SuccessAsync())
                    .OnCustomError(async ex => await SecondTaskMock.Object.CustomErrorAsync())
                    .OnError(async ex => await SecondTaskMock.Object.ErrorAsync())
                    .Always(async () => await SecondTaskMock.Object.AlwaysAsync())
                .Next()
                .ExecuteAllAsync()
                .Await();
        });

        It should_call_execute_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Once);
        It should_not_call_success_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_call_error_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.ErrorAsync(), Times.Once);
        It should_call_always_async_method_on_first_task = () => FirstTaskMock.Verify(x => x.AlwaysAsync(), Times.Once);

        It should_not_call_execute_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.ExecuteAsync(), Times.Never);
        It should_not_call_success_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.SuccessAsync(), Times.Never);
        It should_not_call_custom_error_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.CustomErrorAsync(), Times.Never);
        It should_not_call_error_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.ErrorAsync(), Times.Never);
        It should_not_call_always_async_method_on_second_task = () => SecondTaskMock.Verify(x => x.AlwaysAsync(), Times.Never);

        It should_throw_an_exception = () => Exception.ShouldBeOfExactType<Exception>();
    }

    [Subject("Handler ExecuteAllAsync")]
    public class When_two_tasks_are_assigned_and_there_are_two_different_execute_all_calls_async : Handler_specs
    {
        Establish context = () => Initialize();

        Because of = () =>
        {
            Handler
                .Run(() => FirstTaskMock.Object.ExecuteAsync())
                .Next()
                .Run(() => FirstTaskMock.Object.ExecuteAsync())
                .Next()
                .ExecuteAllAsync();

            Handler
                .Run(() => SecondTaskMock.Object.ExecuteAsync())
                .Next()
                .Run(() => SecondTaskMock.Object.ExecuteAsync())
                .Next()
                .ExecuteAllAsync();
        };

        It should_call_execute_method_on_first_task = () => FirstTaskMock.Verify(x => x.ExecuteAsync(), Times.Exactly(2));
        It should_call_execute_method_on_second_task = () => SecondTaskMock.Verify(x => x.ExecuteAsync(), Times.Exactly(2));
    }
}