using System;

namespace Coolector.Common.Events.Users
{
    public class UserSignedOut : IEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public Guid SessionId { get; }

        protected UserSignedOut()
        {
        }

        public UserSignedOut(Guid requestId, string userId, Guid sessionId)
        {
            RequestId = requestId;
            UserId = userId;
            SessionId = sessionId;
        }
    }
}