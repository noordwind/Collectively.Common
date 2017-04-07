using System;
using System.Linq;
using Autofac;
using Collectively.Common.Security;

namespace Collectively.Common.ServiceClients
{
    public abstract class ServiceClientsModuleBase : Module
    {
        protected void RegisterService<TService, TInterface>(ContainerBuilder builder, string title) where TService : TInterface
        {
            builder.Register(x =>
                {
                    var name = x.Resolve<ServicesSettings>()
                        .Single(s => s.Title == $"{title}-service")
                        .Name;

                    return (TService)Activator.CreateInstance(typeof(TService),
                        new object[] { x.Resolve<IServiceClient>(), name });
                })
                .As<TInterface>()
                .SingleInstance();
        }
    }
}