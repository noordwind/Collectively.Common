using System.Collections.Generic;
using Newtonsoft.Json;

namespace Collectively.Common.Locations
{
    public class LocationResponse
    {
        [JsonProperty(PropertyName = "results")]
        public IEnumerable<LocationResult> Results { get; set; }        
    }
}