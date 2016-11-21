using System;

namespace Coolector.Common.Events.Operations
{
    public class OperationUpdated : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string State { get; }
        public string Code { get; }
        public string Message { get; }
        public DateTime UpdatedAt { get; }

        protected OperationUpdated()
        {
        }

        public OperationUpdated(Guid requestId,
            string userId, string state,
            string code, string message,
            DateTime updatedAt)
        {
            RequestId = requestId;
            UserId = userId;
            State = state;
            Code = code;
            Message = message;
            UpdatedAt = updatedAt;
        }
    }
}