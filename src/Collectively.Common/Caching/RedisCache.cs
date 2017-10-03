using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Common.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Collectively.Common.Caching
{
    public class RedisCache : ICache
    {
        private readonly ILogger Logger = Log.Logger;
        private bool _available;
        private readonly IDatabase _database;
        private readonly RedisSettings _settings;

        public RedisCache(Maybe<RedisDatabase> database, RedisSettings settings)
        {
            _database = database.HasValue ? database.Value.Database : null;
            _settings = settings;
            _available = _database != null && _settings.Enabled;
        }

        public async Task<Maybe<T>> GetAsync<T>(string key) where T : class
            => _available ? Deserialize<T>(await TryExecuteAsync(database 
                => database.StringGetAsync(GetKey(key)))) : default(T);

        public async Task<IEnumerable<T>> GetManyAsync<T>(params string[] keys) where T : class
        {
            if (keys == null || !keys.Any())
            {
                return Enumerable.Empty<T>();
            }
            var results = await TryExecuteAsync<RedisValue[]>(database => 
                database.StringGetAsync(keys.Select(x => (RedisKey)GetKey(x)).ToArray()));
            if (results == null)
            {
                return null;
            }
            var values = new List<T>();
            foreach (var result in results)
            {
                values.Add(Deserialize<T>(result));
            }
            
            return values;
        }

        public async Task AddAsync(string key, object value, TimeSpan? expiry = null)
            => await TryExecuteAsync(database => 
                database.StringSetAsync(GetKey(key), Serialize(value), expiry));

        public async Task AddToSetAsync(string key, string value)
            => await TryExecuteAsync(database => database.SetAddAsync(GetKey(key), value));       

        public async Task AddToSetAsync(string key, object value)
            => await TryExecuteAsync(database => database.SetAddAsync(GetKey(key), Serialize(value)));

        public async Task AddManyToSetAsync(string key, IEnumerable<string> values)
            => await TryExecuteAsync(database => 
                database.SetAddAsync(GetKey(key), values.Select(x => (RedisValue)x).ToArray()));
        
        public async Task<IEnumerable<string>> GetSetAsync(string key)
        {
            var results = await TryExecuteAsync(database => database.SetMembersAsync(GetKey(key)));

            return results?.Select(x => (string)x);
        }

        public async Task<IEnumerable<T>> GetSetAsync<T>(string key)
        {
            var results = await TryExecuteAsync(database => database.SetMembersAsync(GetKey(key)));

            return results?.Select(x => Deserialize<T>(x));
        }

        public async Task RemoveFromSetAsync(string key, string value)
            => await TryExecuteAsync(database => database.SetRemoveAsync(GetKey(key), value));

        public async Task RemoveFromSetAsync(string key, object value)
            => await TryExecuteAsync(database => database.SetRemoveAsync(GetKey(key), Serialize(value)));   

        public async Task<IEnumerable<string>> GetSortedSetAsync(string key, int? limit = null)
        {
            var take = limit.HasValue ? limit.Value : -1;
            var results = await TryExecuteAsync(database => database.SortedSetRangeByRankAsync(GetKey(key), 0, take));

            return results?.Select(x => x.ToString());
        }

        public async Task AddToSortedSetAsync(string key, string value, int score, int? limit = null)
        {
            if(limit >= 0)
            {
                await TryExecuteAsync(database => 
                    database.SortedSetRemoveRangeByRankAsync(GetKey(key), 0, (-1)*limit.Value));
            }
            await TryExecuteAsync(database => database.SortedSetAddAsync(GetKey(key), value, score));
        }

        public async Task RemoveFromSortedSetAsync(string key, string value)
            => await TryExecuteAsync(database => database.SortedSetRemoveAsync(key, value));

        public async Task GeoAddAsync(string key, double longitude, double latitude, string name)
            => await TryExecuteAsync(database => database.GeoAddAsync(GetKey(key), longitude, latitude, name));

        public async Task GeoRemoveAsync(string key, string name)
            => await TryExecuteAsync(database => database.GeoRemoveAsync(GetKey(key), name));

        public async Task<IEnumerable<GeoResult>> GetGeoRadiusAsync(string key, double longitude, double latitude, double radius)
        {
            var results = await TryExecuteAsync<GeoRadiusResult[]>(database => 
                 database.GeoRadiusAsync(GetKey(key), longitude, latitude, radius));

            return results?.Select(x => new GeoResult
            {
                Name = x.Member.ToString(),
                Distance = x.Distance,
                Longitude = x.Position?.Longitude,
                Latitude = x.Position?.Latitude
            });
        }

        public async Task DeleteAsync(string key)
            => await AddAsync(GetKey(key), null, TimeSpan.FromMilliseconds(1));

        private async Task TryExecuteAsync(Func<IDatabase, Task> database)
        {
            try
            {
                await database(_database);
                if (!_available)
                {
                    _available = true;
                    Logger.Information("Redis became available.");
                }
            }
            catch(Exception ex)
            {
                if (_available)
                {
                    _available = false;
                    Logger.Error(ex, "Redis became unavailable.");
                }
            }
        }

        private async Task<T> TryExecuteAsync<T>(Func<IDatabase,Task<T>> database)
        {
            if (!_available)
            {
                return default(T);
            } 
            try
            {
                var result = await database(_database);
                if (!_available)
                {
                    _available = true;
                    Logger.Information("Redis became available.");
                }

                return result;
            }
            catch(Exception ex)
            {
                if (_available)
                {
                    _available = false;
                    Logger.Error(ex, "Redis became unavailable.");
                }
            }
            return default(T);
        }

        private static string GetKey(string key) => key.ToLowerInvariant();

        private static string Serialize<T>(T value) => JsonConvert.SerializeObject(value);

        private static T Deserialize<T>(string serializedObject)
            => serializedObject.Empty() ? default(T) : JsonConvert.DeserializeObject<T>(serializedObject);
    }
}