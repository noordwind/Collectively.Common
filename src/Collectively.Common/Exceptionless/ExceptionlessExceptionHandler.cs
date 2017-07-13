using System;
using Collectively.Common.Services;
using Exceptionless;

namespace Collectively.Common.Exceptionless
{
    public class ExceptionlessExceptionHandler : IExceptionHandler
    {
        private readonly ExceptionlessSettings _settings;

        public ExceptionlessExceptionHandler(ExceptionlessSettings settings)
        {
            _settings = settings;
            if (_settings.Enabled)
            {
                ExceptionlessClient.Default.Configuration.ApiKey = settings.ApiKey;
            }
        }

        public void Handle(Exception exception, params string[] tags)
        {
            if (!_settings.Enabled)
                return;

            exception.ToExceptionless()
                .AddTags(tags)
                .Submit();
        }

        public void Handle(Exception exception, object data,
            string name = "Request details", params string[] tags)
        {
            if (!_settings.Enabled)
                return;

            exception.ToExceptionless()
                .AddObject(data, name)
                .AddTags(tags)
                .Submit();
        }
    }
}