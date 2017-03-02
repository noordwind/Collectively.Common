using System;
using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.Types;
using NLog;

namespace Collectively.Common.ServiceClients.Operations
{
    public class OperationServiceClient : IOperationServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;

        public OperationServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<T>> GetAsync<T>(Guid requestId) where T : class 
        {
            Logger.Debug($"Requesting GetAsync, requestId:{requestId}");
            return await _serviceClient.GetAsync<T>(_settings.Name, $"/operations/{requestId}");
        }

        public async Task<Maybe<dynamic>> GetAsync(Guid requestId)
            => await GetAsync<dynamic>(requestId);
    }
}