using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
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
            var response = await GetResponseAsync(name, endpoint);
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

        public async Task<Maybe<T>> PostAsync<T>(string name, string endpoint, object data) where T : class
        {
            var url = await GetServiceUrlAsync(name);
            var response = await _httpClient.PostAsync(url, endpoint, data);

            return await DeserializeAsync<T>(response);
        }

        private async Task<Maybe<T>> GetDataAsync<T>(string name, string endpoint) where T : class
        {
            var response = await GetResponseAsync(name, endpoint);

            return await DeserializeAsync<T>(response);
        }

        private static async Task<Maybe<T>> DeserializeAsync<T>(Maybe<HttpResponseMessage> response) where T : class
        {
            if(response.HasNoValue)
            {
                return null;
            }
            if (!response.Value.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Value.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        private async Task<Maybe<HttpResponseMessage>> GetResponseAsync(string name, string endpoint)
        {
            var url = await GetServiceUrlAsync(name);
            var settings = _servicesSettings.SingleOrDefault(x => x.Name == name);
            if(settings == null)
            {
                throw new ArgumentNullException(nameof(settings), $"Settings for service: '{name}' were not found.");
            }
            await AuthenticateAsync(url, settings);

            return await RetryGetResponseAsync(url, endpoint, settings);
        }

        private async Task<Maybe<HttpResponseMessage>> RetryGetResponseAsync(string url, string endpoint, ServiceSettings settings)
        {
            var retryNumber = 0;
            var retryCount = settings.RetryCount <= 0 ? 1 : settings.RetryCount;
            var retryDelayMilliseconds = settings.RetryDelayMilliseconds <= 0 ? 200 : settings.RetryDelayMilliseconds;
            while (retryNumber < retryCount)
            {
                try
                {
                    Logger.Debug($"Fetch data from http endpoint: {endpoint}");
                    var response = await _httpClient.GetAsync(url, endpoint);
                    if(response.HasValue)
                    {
                        return response;
                    }
                    await Task.Delay(retryDelayMilliseconds);
                    retryNumber++;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Exception occured while fetching data from endpoint: {endpoint}");
                }
            }

            return null;
        }       

        private async Task AuthenticateAsync(string url, ServiceSettings settings)
        {
            var token = _authenticatedServices[settings.Name];
            if (token.Empty())
            {
                var authenticationToken = await _serviceAuthenticatorClient.AuthenticateAsync(url, new Credentials
                {
                    Username = settings.Username,
                    Password = settings.Password
                });
                if (authenticationToken.HasNoValue)
                {
                    throw new AuthenticationException("Could not get authentication token for service: '{settings.Name}'.");
                }
                token = authenticationToken.Value.Token;
                _authenticatedServices[settings.Name] = token;
            }
            _httpClient.SetAuthorizationHeader(token);
        }

        //TODO: Refactor once Consul will be a part of the solution.
        private async Task<string> GetServiceUrlAsync(string name)
            => await Task.FromResult($"http://{name}");
  }
}