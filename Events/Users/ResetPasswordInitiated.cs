using System;

namespace Coolector.Common.Events.Users
{
    public class ResetPasswordInitiated : IEvent
    {
        public Guid RequestId { get; }
        public string Email { get; }

        protected ResetPasswordInitiated()
        {
        }

        public ResetPasswordInitiated(Guid requestId, string email)
        {
            RequestId = requestId;
            Email = email;
        }
    }
}