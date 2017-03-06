using System;
using System.Collections.Generic;
using Autofac;
using Collectively.Common.Security;
using Collectively.Messages.Events;

namespace Collectively.Common.Services
{
    public class ResourceFactory : IResourceFactory
    {
        private readonly string _service;
        private readonly IDictionary<Type, string> _resources;

        public ResourceFactory(string service, IDictionary<Type, string> resources)
        {
            _service = service;
            _resources = resources;
        }

        public Resource Resolve<T>(params object[] args) where T : class
            => Resource.Create(_service, string.Format(_resources[typeof(T)], args));

        public class Module : Autofac.Module
        {
            private readonly IDictionary<Type, string> _resources;

            public Module(IDictionary<Type, string> resources)
            {
                _resources = resources;           
            }

            protected override void Load(ContainerBuilder builder)
            {
                builder.Register((c, p) => 
                {
                    var settings = c.Resolve<ServiceSettings>();

                    return new ResourceFactory(settings.Name, _resources);
                })
                .As<IResourceFactory>()
                .SingleInstance();
            }
        }
    }
}