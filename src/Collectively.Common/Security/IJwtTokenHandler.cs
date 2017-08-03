using System;
using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public interface IJwtTokenHandler
    {
        Maybe<JwtDetails> Parse(string token);
        Maybe<JwtBasic> Create(string userId, string role, TimeSpan? expiry = null, string state = "active");
        Maybe<string> GetFromAuthorizationHeader(string authorizationHeader);
    }
}