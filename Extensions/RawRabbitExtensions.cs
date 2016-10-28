using System.Reflection;
using Coolector.Common.Commands;
using Coolector.Common.Events;
using RawRabbit;
using RawRabbit.Common;

namespace Coolector.Common.Extensions
{
    public static class RawRabbitExtensions
    {
        public static ISubscription WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(async (msg, context) => await handler.HandleAsync(msg),
                 cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TCommand>())));

        public static ISubscription WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(async (msg, context) => await handler.HandleAsync(msg), 
                cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TEvent>())));

        private static string GetExchangeName<T>() => $"{Assembly.GetEntryAssembly().GetName().Name}/{typeof(T).Name}";
    }
}