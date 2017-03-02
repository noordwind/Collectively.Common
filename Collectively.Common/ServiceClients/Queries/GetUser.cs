using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetUser : IQuery
    {
        public string Id { get; set; }
    }
}