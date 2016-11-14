using System;

namespace Coolector.Common.Commands.Users
{
    public class AuthenticateViaFacebook : ICommand
    {
        public Request Request { get; set; }
        public Guid SessionId { get; set; }
        public string Token { get; set; }
    }
}