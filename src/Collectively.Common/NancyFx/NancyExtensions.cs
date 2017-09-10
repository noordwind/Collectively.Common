using Autofac;
using Collectively.Common.Security;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;

namespace Collectively.Common.NancyFx
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
                headers = context.Request.Headers
            };

        public static void SetupTokenAuthentication(this IPipelines pipelines, IJwtTokenHandler jwtTokenHandler)
            => StatelessAuthentication.Enable(pipelines, Configuration(jwtTokenHandler));            

        private static StatelessAuthenticationConfiguration Configuration(IJwtTokenHandler jwtTokenHandler) 
            => new StatelessAuthenticationConfiguration(ctx =>
            {
                var authToken = jwtTokenHandler.GetFromAuthorizationHeader(ctx.Request.Headers.Authorization);
                if(authToken.HasNoValue)
                {
                    return null;
                }
                var jwt = jwtTokenHandler.Parse(authToken.Value);
                if(jwt.HasNoValue)
                {
                    return null;
                }
                var token = jwt.Value;

                return jwt.HasValue ? new CollectivelyIdentity(token.Subject, 
                    token.Role, token.State, token.Claims) : null;
            });
    }
}