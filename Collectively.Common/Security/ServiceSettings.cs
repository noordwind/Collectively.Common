using System;

namespace Collectively.Common.Security
{
    public class ServiceSettings
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }   
        public TimeSpan? CacheExpiry { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelayMilliseconds { get; set; }     
    }
}