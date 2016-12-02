using System;

namespace Coolector.Common.Events.Users
{
    public class SetNewPasswordRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Reason { get; }
        public string Code { get; }
        public string Email { get; }

        protected SetNewPasswordRejected()
        {
        }

        public SetNewPasswordRejected(Guid requestId, string code, string reason, string email)
        {
            RequestId = requestId;
            Reason = reason;
            Code = code;
            Email = email;
        }
    }
}