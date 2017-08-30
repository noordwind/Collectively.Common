using System;
using Collectively.Common.Types;
using Serilog;
using StackExchange.Redis;

namespace Collectively.Common.Caching
{
    public class RedisDatabaseFactory : IRedisDatabaseFactory
    {
        private static readonly ILogger Logger = Log.Logger;
        private readonly RedisSettings _redisSettings;
        private ConnectionMultiplexer _connectionMultiplexer;

        public RedisDatabaseFactory(RedisSettings redisSettings)
        {
            _redisSettings = redisSettings;
            TryConnect();
        }

        private void TryConnect()
        {
            if (!_redisSettings.Enabled)
            {
                Logger.Information("Connection to Redis server has been skipped (disabled).");

                return;
            }
            try
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisSettings.ConnectionString);
                Logger.Information("Connection to Redis server has been established.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Could not connect to Redis server.");
            }
        }

        public Maybe<RedisDatabase> GetDatabase(int id = -1)
        {
            var database = _connectionMultiplexer?.GetDatabase(id);

            return database == null ? null : new RedisDatabase(database);
        }
    }
}