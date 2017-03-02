using System;
using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetUserSession : IQuery
    {
        public Guid Id { get; set; }
    }
}