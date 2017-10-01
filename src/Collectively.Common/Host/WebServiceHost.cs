using System;
using System.IO;
using System.Linq;
using Autofac;
using Collectively.Messages.Commands;
using Collectively.Messages.Events;
using Collectively.Common.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using RawRabbit;
using Microsoft.Extensions.Configuration;
using Lockbox.Client.Extensions;

namespace Collectively.Common.Host
{
    public class WebServiceHost : IWebServiceHost
    {
        private readonly IWebHost _webHost;

        public WebServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run()
        {
            _webHost.Run();
        }

        public static Builder Create<TStartup>(string name = "", string[] args = null, 
            bool useLockbox = true) where TStartup : class
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = $"Collectively Service: {typeof(TStartup).Namespace.Split('.').Last()}";
            }            
            Console.Title = name;
            var webHost = new WebHostBuilder()
                .UseStartup<TStartup>()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                    if (!useLockbox)
                    {
                        return;
                    }
                    if (env.IsProduction() || env.IsDevelopment())
                    {
                        config.AddLockbox();
                    }
                })
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsEnvironment("local");
                })                
                .Build();
                
            return new Builder(webHost);
        }

        public abstract class BuilderBase
        {
            public abstract WebServiceHost Build();
        }

        public class Builder : BuilderBase
        {
            private IResolver _resolver;
            private IBusClient _bus;
            private readonly IWebHost _webHost;

            public Builder(IWebHost webHost)
            {
                _webHost = webHost;
                _resolver = new DefaultResolver(webHost);
            }

            public Builder UseAutofac(ILifetimeScope scope)
            {
                _resolver = new AutofacResolver(scope);

                return this;
            }

            public BusBuilder UseRabbitMq(string queueName = null)
            {
                _bus = _resolver.Resolve<IBusClient>();

                return new BusBuilder(_webHost, _bus, _resolver, queueName);
            }

            public override WebServiceHost Build()
            {
                return new WebServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private readonly IBusClient _bus;
            private readonly IResolver _resolver;
            private readonly string _queueName;

            public BusBuilder(IWebHost webHost, IBusClient bus, IResolver resolver, string queueName = null)
            {
                _webHost = webHost;
                _bus = bus;
                _resolver = resolver;
                _queueName = queueName;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
            {
                _bus.WithCommandHandlerAsync<TCommand>(_resolver, _queueName);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                _bus.WithEventHandlerAsync<TEvent>(_resolver, _queueName);

                return this;
            }

            public override WebServiceHost Build()
            {
                return new WebServiceHost(_webHost);
            }
        }
    }
}