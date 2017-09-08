using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Collectively.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Collectively.Common.Logging
{
    public static class LoggerExtensions
    {
        public static void AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new SerilogSettings();
            configuration.GetSection("serilog").Bind(settings);
            services.AddSingleton<SerilogSettings>(settings);
            services.AddLogging();
        }

        public static void UseSerilog(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var settings = app.ApplicationServices.GetService<SerilogSettings>();
            if (settings.Level.Empty())
            {
                throw new ArgumentException("Log level can not be empty.", nameof(settings.Level));
            }
            var level = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), settings.Level, true);
            loggerFactory.AddSerilog();
            if (!settings.ElkEnabled)
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Is(level)
                    .CreateLogger();

                return;
            }
            if (settings.ApiUrl.Empty())
            {
                throw new ArgumentException("ELK API URL can not be empty.", nameof(settings.ApiUrl));
            }
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Is(level)
               .WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(settings.ApiUrl))
                {
                    MinimumLogEventLevel = level,
                    AutoRegisterTemplate = true,
                    IndexFormat = settings.IndexFormat.Empty() ? 
                        "logstash-{0:yyyy.MM.dd}" : 
                        settings.IndexFormat,
                    ModifyConnectionSettings = x => 
                        settings.UseBasicAuth ? 
                        x.BasicAuthentication(settings.Username, settings.Password) : 
                        x
                }) 
               .CreateLogger();
        }
    }
}