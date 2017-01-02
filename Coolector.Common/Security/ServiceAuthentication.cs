using Coolector.Common.Extensions;
using Coolector.Common.Types;

namespace Coolector.Common.Security
{
    public class ServiceAuthentication : IServiceAuthentication
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly ServiceSecuritySettings _serviceSecuritySettings;

        public ServiceAuthentication(IJwtTokenHandler jwtTokenHandler,
            JwtTokenSettings jwtTokenSettings, 
            ServiceSecuritySettings serviceSecuritySettings)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _jwtTokenSettings = jwtTokenSettings;
            _serviceSecuritySettings = serviceSecuritySettings;
        }
        
        public Maybe<string> CreateToken(string username, string password)
        {
            if (username.Empty() || password.Empty())
            {
                return null;
            }
            if (username.Equals(_serviceSecuritySettings.Username) && 
                password.Equals(_serviceSecuritySettings.Password))
            {
                return _jwtTokenHandler.Create(username);
            }

            return null;
        }
    }
}