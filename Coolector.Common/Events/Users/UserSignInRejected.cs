using System;

namespace Coolector.Common.Events.Users
{
    public class UserSignInRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Code { get; }
        public string Reason { get; }
        public string Provider { get; }

        protected UserSignInRejected()
        {
        }

        public UserSignInRejected(Guid requestId,
            string userId, string code,
            string reason, string provider)
        {
            RequestId = requestId;
            UserId = userId;
            Code = code;
            Reason = reason;
            Provider = provider;
        }
    }
}