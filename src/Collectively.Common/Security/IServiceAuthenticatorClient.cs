using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public interface IServiceAuthenticatorClient
    {
        Task<Maybe<string>> AuthenticateAsync(string serviceUrl, Credentials credentials);
    }
}