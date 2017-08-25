using System;

namespace Collectively.Common.Security
{
    public class JwtSession : JwtBasic
    {
        public Guid SessionId { get; set; }
        public string Key { get; set; }
    }
}