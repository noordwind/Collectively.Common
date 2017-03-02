using System;
using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetOperation : IQuery
    {
        public Guid RequestId { get; set; }
    }
}