using System.Collections.Generic;
using Newtonsoft.Json;

namespace Collectively.Common.Locations
{
    public class LocationResult
    {
        [JsonProperty(PropertyName = "address_components")]
        public IEnumerable<AddressComponent> AddressComponents { get ; set; }        
    }
}