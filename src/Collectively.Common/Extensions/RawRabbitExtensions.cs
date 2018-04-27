using System.Reflection;
using System.Threading.Tasks;
using Collectively.Common.Host;
using Collectively.Messages.Commands;
using Collectively.Messages.Events;
using RawRabbit;
using RawRabbit.Pipe;

namespace Collectively.Common.Extensions
{
    public static class RawRabbitExtensions
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            IResolver resolver, string name = null) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(msg => resolver.Resolve<ICommandHandler<TCommand>>().HandleAsync(msg),
                ctx => ctx.UseSubscribeConfiguration(cfg => 
                    cfg.FromDeclaredQueue(q => q.WithName(GetExchangeName<TCommand>(name)))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IResolver resolver, string name = null) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(msg => resolver.Resolve<IEventHandler<TEvent>>().HandleAsync(msg),
                ctx => ctx.UseSubscribeConfiguration(cfg => 
                    cfg.FromDeclaredQueue(q => q.WithName(GetExchangeName<TEvent>(name)))));

        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";
    }
}