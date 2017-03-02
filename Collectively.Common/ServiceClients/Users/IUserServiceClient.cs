using System;
using System.Threading.Tasks;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.Types;

namespace Collectively.Common.ServiceClients.Users
{
    public interface IUserServiceClient
    {
        Task<Maybe<T>> IsAvailableAsync<T>(string name) where  T : class;
        Task<Maybe<dynamic>> IsAvailableAsync(string name);
        Task<Maybe<PagedResult<T>>> BrowseAsync<T>(BrowseUsers query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<T>> GetAsync<T>(string userId) where T : class;
        Task<Maybe<dynamic>> GetAsync(string userId);
        Task<Maybe<T>> GetByNameAsync<T>(string name) where T : class;
        Task<Maybe<dynamic>> GetByNameAsync(string name);
        Task<Maybe<T>> GetSessionAsync<T>(Guid id) where T : class;
        Task<Maybe<dynamic>> GetSessionAsync(Guid id);
    }
}