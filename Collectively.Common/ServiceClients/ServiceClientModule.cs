using System;
using System.Linq;
using Autofac;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients.Operations;
using Collectively.Common.ServiceClients.Remarks;
using Collectively.Common.ServiceClients.Statistics;
using Collectively.Common.ServiceClients.Users;

namespace Collectively.Common.ServiceClients
{
    public class ServiceClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomHttpClient>()
                .As<IHttpClient>();

            builder.RegisterType<ServiceClient>()
                .As<IServiceClient>();

            RegisterService<OperationServiceClient, IOperationServiceClient>(builder, "operations");
            RegisterService<RemarkServiceClient, IRemarkServiceClient>(builder, "remarks");
            RegisterService<UserServiceClient, IUserServiceClient>(builder, "users");
            RegisterService<StatisticsServiceClient, IStatisticsServiceClient>(builder, "statistics");
        }

        private void RegisterService<TService, TInterface>(ContainerBuilder builder, string title) where TService : TInterface
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

            builder.Register(x => (TService)Activator.CreateInstance(typeof(TService), 
                new object[]{x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(settingsKey)}))
                .As<TInterface>()
                .SingleInstance();
        }
    }
}