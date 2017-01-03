using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Common.Security
{
    public interface IServiceAuthenticatorClient
    {
        Task<Maybe<string>> AuthenticateAsync(string serviceUrl, Credentials credentials);
    }
}