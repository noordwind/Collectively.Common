using System;

namespace Coolector.Common.Commands.Users
{
    public class Authenticate : ICommand
    {
        public Request Request { get; set; }
        public Guid SessionId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime Expiry { get; set; }
    }
}