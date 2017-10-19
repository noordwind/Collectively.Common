using System;
using Collectively.Common.Extensions;
using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public class ServiceAuthenticatorHost : IServiceAuthenticatorHost
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly ServiceSettings _serviceSettings;

        public ServiceAuthenticatorHost(IJwtTokenHandler jwtTokenHandler,
            JwtTokenSettings jwtTokenSettings, 
            ServiceSettings serviceSettings)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _jwtTokenSettings = jwtTokenSettings;
            _serviceSettings = serviceSettings;
        }
        
        public Maybe<JwtBasic> CreateToken(Credentials credentials)
        {
            if (credentials == null)
            {
                return null;
            }
            if (credentials.Username.Empty() || credentials.Password.Empty())
            {
                return null;
            }
            if (credentials.Username.Equals(_serviceSettings.Username) && 
                credentials.Password.Equals(_serviceSettings.Password))
            {
                var token = _jwtTokenHandler.Create(credentials.Username, string.Empty, 
                    TimeSpan.FromDays(1000));

                return token.Value;
            }

            return null;
        }
    }
}