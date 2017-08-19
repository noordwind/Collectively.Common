using System;
using System.IO;
using Autofac;
using NLog;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;

namespace Collectively.Common.RabbitMq
{
    public static class RabbitMqContainer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Register(ContainerBuilder builder, RawRabbitConfiguration configuration, int retryAttempts = 5)
        {
            var policy = Policy
                .Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .Or<IOException>()
                .WaitAndRetry(retryAttempts, retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Logger.Error(exception, "Can not connect to RabbitMQ. " +
                                                $"Retries: {retryCount}, duration: {timeSpan}");
                    }
                );

            builder.RegisterInstance(configuration).SingleInstance();
            policy.Execute(() => builder
                    .RegisterInstance(RawRabbitFactory.CreateSingleton(new RawRabbitOptions
                    {
                        ClientConfiguration  = configuration
                    }))
                    .As<IBusClient>()
            );
        }
    }
}