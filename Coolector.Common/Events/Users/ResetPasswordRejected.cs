using System;

namespace Coolector.Common.Events.Users
{
    public class ResetPasswordRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Reason { get; }
        public string Code { get; }
        public string Email { get; }

        protected ResetPasswordRejected()
        {
        }

        public ResetPasswordRejected(Guid requestId, string reason, string code, string email)
        {
            RequestId = requestId;
            Reason = reason;
            Code = code;
            Email = email;
        }
    }
}