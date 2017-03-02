using System;
using System.Threading.Tasks;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.Types;
using MongoDB.Driver;

namespace Collectively.Common.ServiceClients.Remarks
{
    public interface IRemarkServiceClient
    {
        Task<Maybe<T>> GetAsync<T>(Guid id) where T : class;
        Task<Maybe<dynamic>> GetAsync(Guid id);
        Task<Maybe<PagedResult<T>>> BrowseCategoriesAsync<T>(BrowseRemarkCategories query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<T>>> BrowseTagsAsync<T>(BrowseRemarkTags query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}