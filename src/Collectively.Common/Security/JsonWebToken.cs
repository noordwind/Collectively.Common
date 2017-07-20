using System.Collections.Generic;

namespace Collectively.Common.Security
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public string Subject { get; set; }
        public string Role { get; set; }
        public long Expires { get; set; }
        public IEnumerable<string> Claims { get; set; }        
    }
}