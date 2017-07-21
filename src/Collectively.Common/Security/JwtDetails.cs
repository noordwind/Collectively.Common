using System.Collections.Generic;
using System.Security.Claims;

namespace Collectively.Common.Security
{
    public class JwtDetails
    {
        public string Subject { get; set; }
        public long Expires { get; set; }
        public IEnumerable<Claim> Claims { get; set; }        
    }
}