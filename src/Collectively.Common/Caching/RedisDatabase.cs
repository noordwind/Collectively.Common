using StackExchange.Redis;

namespace Collectively.Common.Caching
{
    public class RedisDatabase
    {
        public IDatabase Database { get; }

        public RedisDatabase(IDatabase database)
        {
            Database = database;
        }
    }
}