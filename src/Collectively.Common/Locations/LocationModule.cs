using Autofac;

namespace Collectively.Common.Locations
{
    public class LocationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocationService>().As<ILocationService>();
        }
    }
}