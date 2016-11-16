using System;

namespace Coolector.Common.Events.Users
{
    public class UserSignUpRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Reason { get; }
        public string Provider { get; }

        protected UserSignUpRejected()
        {
        }

        public UserSignUpRejected(Guid requestId, string userId,
            string reason, string provider)
        {
            RequestId = requestId;
            UserId = userId;
            Reason = reason;
            Provider = provider;
        }
    }
}