using System;
using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetRemarkPhoto : IQuery
    {
        public Guid Id { get; set; }
    }
}