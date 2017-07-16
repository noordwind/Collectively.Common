using System.Reflection;
using Collectively.Messages.Commands;
using Collectively.Messages.Events;
using RawRabbit;
using RawRabbit.Common;

namespace Collectively.Common.Extensions
{
    public static class RawRabbitExtensions
    {
        public static ISubscription WithCommandHandler<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler, string name = null) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(async (msg, context) => await handler.HandleAsync(msg),
            cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TCommand>(name))));

        public static ISubscription WithEventHandler<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler, string name = null) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(async (msg, context) => await handler.HandleAsync(msg),
            cfg => cfg.WithQueue(q => q.WithName(GetExchangeName<TEvent>(name))));

        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";
    }
}