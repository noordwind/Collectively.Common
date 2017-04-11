using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Newtonsoft.Json;

namespace Collectively.Common.ServiceClients
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Remove("Accept");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<Maybe<HttpResponseMessage>> GetAsync(string url, string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(GetFullAddress(url, endpoint));
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }
            catch (Exception)
            {
            }
            
            return null;
        }

        public async Task<Maybe<HttpResponseMessage>> PostAsync(string url, string endpoint, object data)
        {
            var payload = JsonConvert.SerializeObject(data);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(GetFullAddress(url, endpoint), content);
        }

        private string GetFullAddress(string url, string endpoint)
            => $"{(url.EndsWith("/", StringComparison.CurrentCultureIgnoreCase) ? url : $"{url}/")}{endpoint}";
  }
}