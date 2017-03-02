using System;
using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.Types;
using NLog;

namespace Collectively.Common.ServiceClients.Users
{
    public class UserServiceClient : IUserServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;

        public UserServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<T>> IsAvailableAsync<T>(string name) where T : class
        {
            Logger.Debug($"Requesting IsAvailableAsync, name:{name}");
            return await _serviceClient.GetAsync<T>(_settings.Name, $"users/{name}/available");
        }

        public async Task<Maybe<dynamic>> IsAvailableAsync(string name)
            => await IsAvailableAsync<dynamic>(name);

        public async Task<Maybe<PagedResult<T>>> BrowseAsync<T>(BrowseUsers query) where T : class
        {
            Logger.Debug($"Requesting BrowseAsync, page:{query.Page}, results:{query.Results}");
            return await _serviceClient.GetCollectionAsync<T>(_settings.Name, "users");
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseAsync(BrowseUsers query)
            => await BrowseAsync<dynamic>(query);

        public async Task<Maybe<T>> GetAsync<T>(string userId) where T : class
        {
            Logger.Debug($"Requesting GetAsync, userId:{userId}");
            return await _serviceClient.GetAsync<T>(_settings.Name, $"users/{userId}");
        }

        public async Task<Maybe<dynamic>> GetAsync(string userId)
            => await GetAsync<dynamic>(userId);

        public async Task<Maybe<T>> GetByNameAsync<T>(string name) where T : class
        {
            Logger.Debug($"Requesting GetByNameAsync, name:{name}");
            return await _serviceClient.GetAsync<T>(_settings.Name, $"users/{name}/account");
        }

        public async Task<Maybe<dynamic>> GetByNameAsync(string name)
            => await GetByNameAsync<dynamic>(name);

        public async Task<Maybe<T>> GetSessionAsync<T>(Guid id) where T : class
        {
            Logger.Debug($"Requesting GetSessionAsync, id:{id}");
            return await _serviceClient.GetAsync<T>(_settings.Name, $"user-sessions/{id}");
        }

        public async Task<Maybe<dynamic>> GetSessionAsync(Guid id)
            => await GetSessionAsync<dynamic>(id);
    }
}