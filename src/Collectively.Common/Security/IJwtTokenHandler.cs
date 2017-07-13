using System;

namespace Collectively.Common.Security
{
    public interface IJwtTokenHandler
    {
        string Create(string userId, TimeSpan? expiry = null);
        JwtToken GetFromAuthorizationHeader(string authorizationHeader);
        bool IsValid(JwtToken token);
    }
}