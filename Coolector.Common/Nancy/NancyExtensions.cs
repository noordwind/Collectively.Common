using Nancy;

namespace Coolector.Common.Nancy
{
    public static class NancyExtensions
    {
        public static object ToExceptionData(this NancyContext context)
            => new
            {
                userId = context.CurrentUser?.Identity?.Name ?? string.Empty,
                path = context.Request.Path,
                method = context.Request.Method,
                ipAddress = context.Request.UserHostAddress,
                headers = context.Request.Headers,
            };
    }
}