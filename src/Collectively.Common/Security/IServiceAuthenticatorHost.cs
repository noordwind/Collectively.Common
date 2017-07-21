using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public interface IServiceAuthenticatorHost
    {
         Maybe<JwtBasic> CreateToken(Credentials credentials);
    }
}