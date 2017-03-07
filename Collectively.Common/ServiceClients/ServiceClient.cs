using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Queries;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Messages.Events;
using Newtonsoft.Json;
using NLog;

namespace Collectively.Common.ServiceClients
{
    public class ServiceClient : IServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IDictionary<string, string> _authenticatedServices = new Dictionary<string, string>();
        private ServicesSettings _servicesSettings;
        private readonly IHttpClient _httpClient;
        private readonly IServiceAuthenticatorClient _serviceAuthenticatorClient;

        public ServiceClient(IHttpClient httpClient, IServiceAuthenticatorClient serviceAuthenticatorClient,
            ServicesSettings servicesSettings)
        {
            _httpClient = httpClient;
            _serviceAuthenticatorClient = serviceAuthenticatorClient;
            _servicesSettings = servicesSettings;
            foreach(var service in _servicesSettings.Where(x => x.Name.NotEmpty()))
            {
                _authenticatedServices[service.Name] = string.Empty;
            }
        }

        public async Task<Maybe<T>> GetAsync<T>(Resource resource) where T : class
            => await GetAsync<T>(resource.Service, resource.Endpoint);

        public async Task<Maybe<T>> GetAsync<T>(string name, string endpoint) where T : class
        {
            var data = await GetDataAsync<T>(name, endpoint);
            if (data.HasNoValue)
            {
                return new Maybe<T>();
            }

            return data;
        }

        public async Task<Maybe<dynamic>> GetAsync(Resource resource)
            => await GetAsync(resource.Service, resource.Endpoint);

        public async Task<Maybe<dynamic>> GetAsync(string name, string endpoint)
            => await GetAsync<dynamic>(name, endpoint);

        public async Task<Maybe<Stream>> GetStreamAsync(Resource resource)
            => await GetStreamAsync(resource.Service, resource.Endpoint);

        public async Task<Maybe<Stream>> GetStreamAsync(string name, string endpoint)
        {
            var url = await GetServiceUrlAsync(name);
            var response = await _httpClient.GetAsync(url, endpoint);
            if (response.HasNoValue)
            {
                return new Maybe<Stream>();
            }

            return await response.Value.Content.ReadAsStreamAsync();
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(Resource resource) where T : class
            => await GetCollectionAsync<T>(resource.Service, resource.Endpoint);

        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string name, string endpoint) where T : class
        {
            var data = await GetDataAsync<IEnumerable<T>>(name, endpoint);
            if (data.HasNoValue)
            {
                return new Maybe<PagedResult<T>>();
            }

            return data.Value.PaginateWithoutLimit();
        }

        public async Task<Maybe<PagedResult<dynamic>>> GetCollectionAsync(Resource resource)
            => await GetCollectionAsync(resource.Service, resource.Endpoint);

        public Task<Maybe<PagedResult<dynamic>>> GetCollectionAsync(string name, string endpoint)
            => GetCollectionAsync<dynamic>(name, endpoint);

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TQuery, TResult>(TQuery query, Resource resource) 
            where TQuery : class, IPagedQuery where TResult : class
            => await GetFilteredCollectionAsync<TQuery, TResult>(query, resource.Service, resource.Endpoint);

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TQuery, TResult>(TQuery query, 
            string name, string endpoint) 
            where TQuery : class, IPagedQuery where TResult : class
        {
            var queryString = endpoint.ToQueryString(query);

            return await GetCollectionAsync<TResult>(name, queryString);
        }

        public async Task<Maybe<PagedResult<dynamic>>> GetFilteredCollectionAsync<TQuery>(TQuery query, Resource resource) 
            where TQuery : class, IPagedQuery
            => await GetFilteredCollectionAsync<TQuery>(query, resource.Service, resource.Endpoint);

        public Task<Maybe<PagedResult<dynamic>>> GetFilteredCollectionAsync<TQuery>(
            TQuery query, string name, string endpoint) where TQuery : class, IPagedQuery
            => GetFilteredCollectionAsync<TQuery, dynamic>(query, name, endpoint);

        private async Task<Maybe<T>> GetDataAsync<T>(string name, string endpoint) where T : class
        {
            var url = await GetServiceUrlAsync(name);
            var token = _authenticatedServices[name];
            if (token.Empty())
            {
                var settings = _servicesSettings.Single(x => x.Name == name);
                var authenticationToken = await _serviceAuthenticatorClient.AuthenticateAsync(url, new Credentials
                {
                    Username = settings.Username,
                    Password = settings.Password
                });
                if (authenticationToken.HasNoValue)
                {
                    Logger.Error($"Could not get authentication token for service: '{settings.Name}'.");

                    return null;
                }
                token = authenticationToken.Value;
                _authenticatedServices[name] = token;
            }

            _httpClient.SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync(url, endpoint);
            if (response.HasNoValue)
            {
                return new Maybe<T>();
            }

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }

        //TODO: Refactor once Consul will be a part of the solution.
        private async Task<string> GetServiceUrlAsync(string name)
            => await Task.FromResult($"http://{name}");
    }
}