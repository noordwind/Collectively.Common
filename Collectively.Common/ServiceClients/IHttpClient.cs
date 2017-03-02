using System.Net.Http;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.ServiceClients
{
    public interface IHttpClient
    {
        void SetAuthorizationHeader(string token);
        Task<Maybe<HttpResponseMessage>> GetAsync(string url, string endpoint);
    }
}