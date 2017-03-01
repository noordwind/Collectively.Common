using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public interface IServiceAuthenticatorHost
    {
         Maybe<string> CreateToken(Credentials credentials);
    }
}