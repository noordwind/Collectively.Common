using Autofac;

namespace Collectively.Common.ServiceClients
{
    public class ServiceClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomHttpClient>()
                .As<IHttpClient>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ServiceClient>()
                .As<IServiceClient>()
                .InstancePerLifetimeScope();
        }
    }
}