using Coolector.Common.Types;

namespace Coolector.Common.Security
{
    public interface IServiceAuthentication
    {
         Maybe<string> CreateToken(string username, string password);
    }
}