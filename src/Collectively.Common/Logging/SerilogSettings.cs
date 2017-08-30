namespace Collectively.Common.Logging
{
    public class SerilogSettings
    {
        public string Level { get; set; }
        public bool ElkEnabled { get; set; }
        public string ApiUrl { get; set; }
        public bool UseBasicAuth { get; set ;}
        public string Username { get; set; }
        public string Password { get; set; }
    }
}