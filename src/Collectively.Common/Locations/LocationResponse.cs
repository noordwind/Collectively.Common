using System.Collections.Generic;
using Newtonsoft.Json;

namespace Collectively.Common.Locations
{
    public class LocationResponse
    {
        [JsonProperty(PropertyName = "formatted_address")]
        public string FormattedAddress { get; set; } 

        [JsonProperty(PropertyName = "results")]
        public IEnumerable<LocationResult> Results { get; set; }        
    }
}