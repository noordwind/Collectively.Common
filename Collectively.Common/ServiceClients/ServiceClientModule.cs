using System.Linq;
using Autofac;
using Collectively.Common.Security;

namespace Collectively.Common.ServiceClients
{
    public class ServiceClientModule : Module
    {
        private static readonly string[] Services = new []{"mailing", "medium", "operations", 
            "remarks", "statistics", "storage", "supervisor", "users"};

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomHttpClient>()
                .As<IHttpClient>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ServiceClient>()
                .As<IServiceClient>()
                .InstancePerLifetimeScope();

            foreach(var service in Services)
            {
                RegisterSettings(builder, service);
            }
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