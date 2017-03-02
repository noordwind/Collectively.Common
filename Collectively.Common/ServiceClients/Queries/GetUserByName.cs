using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}