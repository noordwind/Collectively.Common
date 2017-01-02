namespace Coolector.Common.Security
{
    public class JwtTokenSettings
    {
        public string SecretKey { get; set; }
        public int ExpiryDays { get; set; }
    }
}