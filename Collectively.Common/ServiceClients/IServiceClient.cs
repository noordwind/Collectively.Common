using System.IO;
using System.Threading.Tasks;
using Collectively.Common.Queries;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Messages.Events;

namespace Collectively.Common.ServiceClients
{
    public interface IServiceClient
    {
        void SetSettings(ServiceSettings serviceSettings);
        Task<Maybe<T>> GetAsync<T>(Resource resource) 
            where T : class;
        Task<Maybe<T>> GetAsync<T>(string name, string endpoint) 
            where T : class;
        Task<Maybe<dynamic>> GetAsync(Resource resource);
        Task<Maybe<dynamic>> GetAsync(string name, string endpoint);
        Task<Maybe<Stream>> GetStreamAsync(Resource resource);
        Task<Maybe<Stream>> GetStreamAsync(string name, string endpoint);
        Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(Resource resource) 
            where T : class;    
        Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string name, string endpoint) 
            where T : class;        
        Task<Maybe<PagedResult<dynamic>>> GetCollectionAsync(Resource resource);
        Task<Maybe<PagedResult<dynamic>>> GetCollectionAsync(string name, string endpoint);
        Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TQuery,TResult>(TQuery query, 
            Resource resource)
            where TResult : class where TQuery : class, IPagedQuery;
        Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TQuery,TResult>(TQuery query, 
            string name, string endpoint)
            where TResult : class where TQuery : class, IPagedQuery;
        Task<Maybe<PagedResult<dynamic>>> GetFilteredCollectionAsync<TQuery>(TQuery query,
            Resource resource)
            where TQuery : class, IPagedQuery;
        Task<Maybe<PagedResult<dynamic>>> GetFilteredCollectionAsync<TQuery>(TQuery query,
            string name, string endpoint)
            where TQuery : class, IPagedQuery;
    }
}