using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.Types;
using NLog;

namespace Collectively.Common.ServiceClients.Statistics
{
    public class StatisticsServiceClient : IStatisticsServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;
        private readonly string UserStatisticsEndpoint = "statistics/users";
        private readonly string RemarkStatisticsEndpoint = "statistics/remarks";
        private readonly string CategoryStatisticsEndpoint = "statistics/categories";
        private readonly string TagStatisticsEndpoint = "statistics/tags";

        public StatisticsServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<PagedResult<T>>> BrowseUserStatisticsAsync<T>(BrowseUserStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting BrowseReportersAsync, page:{query.Page}, results:{query.Results}");
            var queryString = UserStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<T>(_settings.Name, queryString);
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
            => await BrowseUserStatisticsAsync<dynamic>(query);

        public async Task<Maybe<T>> GetUserStatisticsAsync<T>(GetUserStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting GetUserStatisticsAsync, userId:{query.Id}");
            var endpoint = $"{UserStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<T>(_settings.Name, endpoint);
        }

        public async Task<Maybe<dynamic>> GetUserStatisticsAsync(GetUserStatistics query)
            => await GetUserStatisticsAsync<dynamic>(query);

        public async Task<Maybe<PagedResult<T>>> BrowseRemarkStatisticsAsync<T>(BrowseRemarkStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting BrowseRemarkStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = RemarkStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<T>(_settings.Name, queryString);
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
            => await BrowseRemarkStatisticsAsync<dynamic>(query);

        public async Task<Maybe<T>> GetRemarkStatisticsAsync<T>(GetRemarkStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting GetRemarkStatisticsAsync, remarkId:{query.Id}");
            var endpoint = $"{RemarkStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<T>(_settings.Name, endpoint);
        }

        public async Task<Maybe<dynamic>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
            => await GetRemarkStatisticsAsync<dynamic>(query);

        public async Task<Maybe<T>> GetRemarksCountStatisticsAsync<T>(GetRemarksCountStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting GetRemarksCountStatisticsAsync, from:{query.From}, to:{query.To}");
            var endpoint = $"{RemarkStatisticsEndpoint}/general".ToQueryString(query);
            return await _serviceClient
                .GetAsync<T>(_settings.Name, endpoint);
        }

        public async Task<Maybe<dynamic>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query)
            => await GetRemarksCountStatisticsAsync<dynamic>(query);

        public async Task<Maybe<PagedResult<T>>> BrowseCategoryStatisticsAsync<T>(BrowseCategoryStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting BrowseCategoryStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = CategoryStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<T>(_settings.Name, queryString);
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query)
            => await BrowseCategoryStatisticsAsync<dynamic>(query);

        public async Task<Maybe<PagedResult<T>>> BrowseTagStatisticsAsync<T>(BrowseTagStatistics query)
            where T : class
        {
            Logger.Debug($"Requesting BrowseTagStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = TagStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<T>(_settings.Name, queryString);
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseTagStatisticsAsync(BrowseTagStatistics query)
            => await BrowseTagStatisticsAsync<dynamic>(query);
    }
}