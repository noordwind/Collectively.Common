using System.Reflection;
using System.Threading.Tasks;
using Collectively.Messages.Commands;
using Collectively.Messages.Events;
using RawRabbit;
using RawRabbit.Pipe;

namespace Collectively.Common.Extensions
{
    public static class RawRabbitExtensions
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler, string name = null) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(msg => handler.HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(cfg => 
                    cfg.FromDeclaredQueue(q => q.WithName(GetExchangeName<TCommand>(name)))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler, string name = null) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(cfg => 
                    cfg.FromDeclaredQueue(q => q.WithName(GetExchangeName<TEvent>(name)))));

        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";
    }
}