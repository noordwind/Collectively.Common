using System;

namespace Coolector.Common.Events.Users
{
    public class UserNameChanged : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string NewName { get; }

        protected UserNameChanged()
        {
        }

        public UserNameChanged(Guid requestId, string userId, string newName)
        {
            RequestId = requestId;
            UserId = userId;
            NewName = newName;
        }
    }
}