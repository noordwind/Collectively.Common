using Collectively.Common.Types;

namespace Collectively.Common.Caching
{
    public interface IRedisDatabaseFactory
    {
        Maybe<RedisDatabase> GetDatabase(int id = -1);
    }
}