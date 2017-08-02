using System;
using System.Net.Http;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Types;
using Newtonsoft.Json;

namespace Collectively.Common.Locations
{
    public class LocationService : ILocationService
    {
        private static readonly Uri ApiUrl = new Uri("https://maps.googleapis.com/maps/api/geocode/json");
        private readonly HttpClient _client;
        private readonly LocationSettings _settings;
        
        public LocationService(LocationSettings settings)
        {
            _settings = settings;
            _client = new HttpClient
            {
                BaseAddress = ApiUrl
            };
        }

        public async Task<Maybe<LocationResponse>> GetAsync(string address)
        => await GetAsync(address, 0, 0);

        public async Task<Maybe<LocationResponse>> GetAsync(double latitude, double longitude)
        => await GetAsync(string.Empty, latitude, longitude);

        public async Task<Maybe<LocationResponse>> GetAsync(string address, double latitude, double longitude)
        {
            var query = address.Empty() ? $"latlng={latitude},{longitude}" : $"address={address}";
            var queryWithKey = _settings.ApiKey.Empty() ? query : $"{query}&key={_settings.ApiKey}";
            var response = await _client.GetAsync($"?{queryWithKey}");
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<LocationResponse>(content));
        }
    }
}