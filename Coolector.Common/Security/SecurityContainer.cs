using Autofac;
using Coolector.Common.Extensions;
using Microsoft.Extensions.Configuration;

namespace Coolector.Common.Security
{
    public static class SecurityContainer
    {
        public static void Register(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<JwtTokenHandler>()
                .As<IJwtTokenHandler>()
                .SingleInstance();
            
            builder.RegisterType<ServiceAuthentication>()
                .As<IServiceAuthentication>()
                .SingleInstance();

            builder.RegisterInstance(configuration.GetSettings<JwtTokenSettings>())
                .SingleInstance();

            builder.RegisterInstance(configuration.GetSettings<SecuredServiceSettings>())
                .SingleInstance();

            builder.RegisterInstance(configuration.GetSettings<SecuredServicesSettings>())
                .SingleInstance();

            builder.RegisterInstance(configuration.GetSettings<ServiceSecuritySettings>())
                .SingleInstance();
        }
    }
}