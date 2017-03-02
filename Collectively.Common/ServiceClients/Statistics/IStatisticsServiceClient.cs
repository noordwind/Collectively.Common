using System.Threading.Tasks;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.Types;

namespace Collectively.Common.ServiceClients.Statistics
{
    public interface IStatisticsServiceClient
    {
        Task<Maybe<PagedResult<T>>> BrowseUserStatisticsAsync<T>(BrowseUserStatistics query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseUserStatisticsAsync(BrowseUserStatistics query);
        Task<Maybe<T>> GetUserStatisticsAsync<T>(GetUserStatistics query) where T : class;
        Task<Maybe<dynamic>> GetUserStatisticsAsync(GetUserStatistics query);
        Task<Maybe<PagedResult<T>>> BrowseRemarkStatisticsAsync<T>(BrowseRemarkStatistics query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query);
        Task<Maybe<T>> GetRemarkStatisticsAsync<T>(GetRemarkStatistics query) where T : class;
        Task<Maybe<dynamic>> GetRemarkStatisticsAsync(GetRemarkStatistics query);
        Task<Maybe<T>> GetRemarksCountStatisticsAsync<T>(GetRemarksCountStatistics query) where T : class;
        Task<Maybe<dynamic>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query);
        Task<Maybe<PagedResult<T>>> BrowseCategoryStatisticsAsync<T>(BrowseCategoryStatistics query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query);
        Task<Maybe<PagedResult<T>>> BrowseTagStatisticsAsync<T>(BrowseTagStatistics query) where T : class;
        Task<Maybe<PagedResult<dynamic>>> BrowseTagStatisticsAsync(BrowseTagStatistics query);
    }
}