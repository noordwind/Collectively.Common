using System;

namespace Coolector.Common.Commands.Users
{
    public class SignOut : IAuthenticatedCommand
    {
        public Request Request { get; set; }
        public Guid SessionId { get; set; }
        public string UserId { get; set; }
    }
}