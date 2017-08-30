using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.Caching
{
    public interface ICache
    {
        Task<Maybe<T>> GetAsync<T>(string key) where T : class;
        Task<IEnumerable<T>> GetManyAsync<T>(params string[] keys) where T : class;
        Task AddAsync(string key, object value, TimeSpan? expiry = null);
        Task GeoAddAsync(string key, double longitude, double latitude, string name);
        Task GeoRemoveAsync(string key, string name);
        Task<IEnumerable<GeoResult>> GetGeoRadiusAsync(string key, double longitude, double latitude, double radius);
        Task DeleteAsync(string key);        
    }
}