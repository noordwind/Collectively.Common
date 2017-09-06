using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Common.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Collectively.Common.Caching
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _database;
        private readonly RedisSettings _settings;
        private bool Available => _database != null && _settings.Enabled;

        public RedisCache(Maybe<RedisDatabase> database, RedisSettings settings)
        {
            _database = database.HasValue ? database.Value.Database : null;
            _settings = settings;
        }

        public async Task<Maybe<T>> GetAsync<T>(string key) where T : class
            => Available ? Deserialize<T>(await _database.StringGetAsync(GetKey(key))) : default(T);

        public async Task<IEnumerable<T>> GetManyAsync<T>(params string[] keys) where T : class
        {
            if (!Available)
            {
                return Enumerable.Empty<T>();
            }
            if (keys == null || !keys.Any())
            {
                return Enumerable.Empty<T>();
            }
            var results = await _database.StringGetAsync(keys.Select(x => (RedisKey)GetKey(x)).ToArray());
            var values = new List<T>();
            foreach (var result in results)
            {
                values.Add(Deserialize<T>(result));
            }
            
            return values;
        }

        public async Task AddAsync(string key, object value, TimeSpan? expiry = null)
        {
            if (!Available)
            {
                return;
            }
            await _database.StringSetAsync(GetKey(key), Serialize(value), expiry);
        }

        public async Task AddToSetAsync(string key, string value)
            => await _database.SetAddAsync(GetKey(key), value);

        public async Task AddToSetAsync(string key, object value)
            => await _database.SetAddAsync(GetKey(key), Serialize(value));

        public async Task AddManyToSetAsync(string key, IEnumerable<string> values)
            => await _database.SetAddAsync(GetKey(key), values.Select(x => (RedisValue)x).ToArray());

        
        public async Task<IEnumerable<string>> GetSetStringsAsync<T>(string key)
        {
            var results = await _database.SetMembersAsync(GetKey(key));

            return results.Select(x => (string)x);
        }

        public async Task<IEnumerable<T>> GetSetAsync<T>(string key)
        {
            var results = await _database.SetMembersAsync(GetKey(key));

            return results.Select(x => Deserialize<T>(x));
        }

        public async Task RemoveFromSetAsync(string key, string value)
            => await _database.SetRemoveAsync(GetKey(key), value);

        public async Task RemoveFromSetAsync(string key, object value)
            => await _database.SetRemoveAsync(GetKey(key), Serialize(value));

        public async Task<IEnumerable<string>> GetSortedSetAsync(string key, int? limit = null)
        {
            var take = limit.HasValue ? limit.Value : -1;
            var results = await _database.SortedSetRangeByRankAsync(GetKey(key), 0, take);

            return results.Select(x => x.ToString());
        }

        public async Task AddToSortedSetAsync(string key, string value, int score, int? limit = null)
        {
            if(limit >= 0)
            {
                await _database.SortedSetRemoveRangeByRankAsync(GetKey(key), 0, (-1)*limit.Value);
            }
            await _database.SortedSetAddAsync(GetKey(key), value, score);
        }

        public async Task RemoveFromSortedSetAsync(string key, string value)
            => await _database.SortedSetRemoveAsync(key, value);

        public async Task GeoAddAsync(string key, double longitude, double latitude, string name)
        {
            if (!Available)
            {
                return;
            }
            await _database.GeoAddAsync(GetKey(key), longitude, latitude, name);
        }

        public async Task GeoRemoveAsync(string key, string name)
        {
            if (!Available)
            {
                return;
            }
            await _database.GeoRemoveAsync(GetKey(key), name);
        }

        public async Task<IEnumerable<GeoResult>> GetGeoRadiusAsync(string key, double longitude, double latitude, double radius)
        {
            if (!Available)
            {
                return Enumerable.Empty<GeoResult>();
            }
            var results = await _database.GeoRadiusAsync(GetKey(key), longitude, latitude, radius);
            if (results == null)
            {
                return Enumerable.Empty<GeoResult>();
            }

            return results.Select(x => new GeoResult
            {
                Name = x.Member.ToString(),
                Distance = x.Distance,
                Longitude = x.Position?.Longitude,
                Latitude = x.Position?.Latitude
            });
        }

        public async Task DeleteAsync(string key)
        {
            if (!Available)
            {
                return;
            }
            await AddAsync(GetKey(key), null, TimeSpan.FromMilliseconds(1));
        }

        private static string GetKey(string key) => key.ToLowerInvariant();

        private static string Serialize<T>(T value) => JsonConvert.SerializeObject(value);

        private static T Deserialize<T>(string serializedObject)
            => serializedObject.Empty() ? default(T) : JsonConvert.DeserializeObject<T>(serializedObject);
    }
}