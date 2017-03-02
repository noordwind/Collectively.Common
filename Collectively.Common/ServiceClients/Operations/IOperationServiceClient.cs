using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.ServiceClients.Operations
{
    public interface IOperationServiceClient
    {
        Task<Maybe<T>> GetAsync<T>(Guid requestId) where T : class;
        Task<Maybe<dynamic>> GetAsync(Guid requestId);
    }
}