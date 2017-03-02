using System;
using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}