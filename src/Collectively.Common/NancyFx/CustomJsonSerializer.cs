using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Collectively.Common.NancyFx
{
    public sealed class CustomJsonSerializer : JsonSerializer
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public CustomJsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.Indented;
            DateFormatString = DateTimeFormat;
            Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter
            {
                AllowIntegerValues = true,
                CamelCaseText = true
            });
        }
    }
}