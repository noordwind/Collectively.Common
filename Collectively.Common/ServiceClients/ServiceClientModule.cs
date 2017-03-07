using System.Linq;
using Autofac;
using Collectively.Common.Security;

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

            RegisterSettings(builder, "operations");
            RegisterSettings(builder, "remarks");
            RegisterSettings(builder, "users");
            RegisterSettings(builder, "statistics");
        }

        private void RegisterSettings(ContainerBuilder builder, string title)
        {
            var settingsKey = $"{title}-settings";
            builder.Register(x => 
            {
                var settings = x.Resolve<ServicesSettings>()
                                .FirstOrDefault(s => s.Title == $"{title}-service");

                return settings ?? new ServiceSettings();
            })
            .Named<ServiceSettings>(settingsKey)
            .SingleInstance();
        }
    }
}