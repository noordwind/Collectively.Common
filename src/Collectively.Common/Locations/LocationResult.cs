using System.Collections.Generic;
using Newtonsoft.Json;

namespace Collectively.Common.Locations
{
    public class LocationResult
    {
        [JsonProperty(PropertyName = "formatted_address")]
        public string FormattedAddress { get; set; } 

        [JsonProperty(PropertyName = "address_components")]
        public IEnumerable<AddressComponent> AddressComponents { get ; set; }        
    }
}