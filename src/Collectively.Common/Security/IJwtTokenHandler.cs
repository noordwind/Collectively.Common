using System;
using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public interface IJwtTokenHandler
    {
        Maybe<JsonWebToken> Parse(string token);
        Maybe<JsonWebToken> Create(string userId, string role, TimeSpan? expiry = null);
        Maybe<string> GetFromAuthorizationHeader(string authorizationHeader);
    }
}