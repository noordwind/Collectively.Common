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
        
        public Maybe<string> CreateToken(Credentials credentials)
        {
            if (credentials == null)
            {
                return null;
            }
            if (credentials.Username.Empty() || credentials.Password.Empty())
            {
                return null;
            }
            if (credentials.Username.Equals(_serviceSecuritySettings.Username) && 
                credentials.Password.Equals(_serviceSecuritySettings.Password))
            {
                return _jwtTokenHandler.Create(credentials.Username);
            }

            return null;
        }
    }
}