namespace Collectively.Common.Security
{
    public class JwtTokenSettings
    {
        public string SecretKey { get; set; }
        public int ExpiryDays { get; set; }
        public string Issuer { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool UseRsa { get; set; }
        public bool UseRsaFilePath { get; set; }
        public string RsaPrivateKeyXML { get; set; }
        public string RsaPublicKeyXML { get; set; }
    }
}