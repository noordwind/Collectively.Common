using System;

namespace Coolector.Common.Events.Users
{
    public class UserSignedIn : IEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public string Provider { get; set; }

        protected UserSignedIn()
        {
        }

        public UserSignedIn(Guid requestId, string userId, string email,
            string name, string provider)
        {
            RequestId = requestId;
            UserId = userId;
            Email = email;
            Name = name;
            Provider = provider;
        }
    }
}