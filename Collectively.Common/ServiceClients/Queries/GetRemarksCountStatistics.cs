using System;
using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetRemarksCountStatistics : IQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}