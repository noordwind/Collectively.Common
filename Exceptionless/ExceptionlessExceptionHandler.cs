using System;
using Coolector.Common.Services;
using Exceptionless;
using Nancy;

namespace Coolector.Common.Exceptionless
{
    public class ExceptionlessExceptionHandler : IExceptionHandler
    {
        private readonly ExceptionlessSettings _settings;

        public ExceptionlessExceptionHandler(ExceptionlessSettings settings)
        {
            _settings = settings;
        }

        public void Handle(Exception exception, NancyContext context,
            string name = "Request details", params string[] tags)
        {
            if (!_settings.Enabled)
                return;

            exception.ToExceptionless()
                .AddObject(new
                {
                    userId = context.CurrentUser?.Identity?.Name ?? string.Empty,
                    path = context.Request.Path,
                    method = context.Request.Method,
                    ipAddress = context.Request.UserHostAddress,
                    headers = context.Request.Headers,
                }, name)
                .AddTags(tags)
                .Submit();
        }
    }
}