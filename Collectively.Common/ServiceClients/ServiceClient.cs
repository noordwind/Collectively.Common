using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Queries;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Newtonsoft.Json;
using NLog;

namespace Collectively.Common.ServiceClients
{
    public class ServiceClient : IServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _isAuthenticated = false;
        private ServiceSettings _serviceSettings;
        private readonly IHttpClient _httpClient;
        private readonly IServiceAuthenticatorClient _serviceAuthenticatorClient;

        public ServiceClient(IHttpClient httpClient, IServiceAuthenticatorClient serviceAuthenticatorClient)
        {
            _httpClient = httpClient;
            _serviceAuthenticatorClient = serviceAuthenticatorClient;
        }

        public void SetSettings(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public async Task<Maybe<T>> GetAsync<T>(string name, string endpoint) where T : class
        {
            var data = await GetDataAsync<T>(name, endpoint);
            if (data.HasNoValue)
                return new Maybe<T>();

            return data;
        }

        public Task<Maybe<dynamic>> GetAsync(string name, string endpoint)
            => GetAsync<dynamic>(name, endpoint);

        public async Task<Maybe<Stream>> GetStreamAsync(string name, string endpoint)
        {
            var response = await _httpClient.GetAsync(GetUrl(name), endpoint);
            if (response.HasNoValue)
                return new Maybe<Stream>();

            return await response.Value.Content.ReadAsStreamAsync();
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string name, string endpoint) where T : class
        {
            var data = await GetDataAsync<IEnumerable<T>>(name, endpoint);
            if (data.HasNoValue)
                return new Maybe<PagedResult<T>>();

            return data.Value.PaginateWithoutLimit();
        }

        public Task<Maybe<PagedResult<dynamic>>> GetCollectionAsync(string name, string endpoint)
            => GetCollectionAsync<dynamic>(name, endpoint);

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TQuery, TResult>(TQuery query, 
            string name, string endpoint) 
            where TQuery : class, IPagedQuery where TResult : class
        {
            var queryString = endpoint.ToQueryString(query);

            return await GetCollectionAsync<TResult>(name, queryString);
        }

        public Task<Maybe<PagedResult<dynamic>>> GetFilteredCollectionAsync<TQuery>(
            TQuery query, string name, string endpoint) where TQuery : class, IPagedQuery
            => GetFilteredCollectionAsync<TQuery, dynamic>(query, name, endpoint);

        private async Task<Maybe<T>> GetDataAsync<T>(string name, string endpoint) where T : class
        {
            var url = GetUrl(name);
            if (!_isAuthenticated && _serviceSettings != null)
            {
                var token = await _serviceAuthenticatorClient.AuthenticateAsync(url, new Credentials
                {
                    Username = _serviceSettings.Username,
                    Password = _serviceSettings.Password
                });
                if (token.HasNoValue)
                {
                    Logger.Error($"Could not get authentication token for service: '{_serviceSettings.Name}'.");

                    return null;
                }

                _httpClient.SetAuthorizationHeader(token.Value);
                _isAuthenticated = true;
            }

            var response = await _httpClient.GetAsync(url, endpoint);
            if (response.HasNoValue)
                return new Maybe<T>();

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }

        private static string GetUrl(string name) => $"http://{name}";
    }
}