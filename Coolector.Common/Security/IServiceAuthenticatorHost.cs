using Coolector.Common.Types;

namespace Coolector.Common.Security
{
    public interface IServiceAuthenticatorHost
    {
         Maybe<string> CreateToken(Credentials credentials);
    }
}