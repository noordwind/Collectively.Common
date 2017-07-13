namespace Collectively.Common.Security
{
    public class JwtToken
    {
        public string Sub { get; set; }
        public long Exp { get; set; }
    }
}