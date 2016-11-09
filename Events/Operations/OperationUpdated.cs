using System;

namespace Coolector.Common.Events.Operations
{
    public class OperationUpdated : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string Name { get; }
        public string UserId { get; }
        public string State { get; }
        public DateTime UpdatedAt { get; }
        public string Message { get; }

        protected OperationUpdated()
        {
        }

        public OperationUpdated(Guid requestId,
            string name,
            string userId, string state,
            DateTime updatedAt, 
            string message)
        {
            RequestId = requestId;
            Name = name;
            UserId = userId;
            State = state;
            UpdatedAt = updatedAt;
            Message = message;
        }
    }
}