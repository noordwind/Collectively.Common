using Collectively.Common.Queries;

namespace Collectively.Common.ServiceClients.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}