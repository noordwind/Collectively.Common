using System;

namespace Coolector.Common.Events.Users
{
    public class ChangePasswordRejected: IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Code { get; }
        public string Reason { get; }

        protected ChangePasswordRejected()
        {
        }

        public ChangePasswordRejected(Guid requestId,
            string userId, string code,
            string reason)
        {
            RequestId = requestId;
            UserId = userId;
            Code = code;
            Reason = reason;
        }
    }
}