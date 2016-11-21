using System;

namespace Coolector.Common.Events.Operations
{
    public class OperationCreated : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string Name { get; }
        public string UserId { get; }
        public string Origin { get; }
        public string Resource { get; }
        public string State { get; }
        public string Code { get; }
        public string Message { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }

        protected OperationCreated()
        {
        }

        public OperationCreated(Guid requestId, string name,
            string userId, string origin, string resource,
            string state, string code, string message,
            DateTime createdAt, DateTime updatedAt)
        {
            RequestId = requestId;
            Name = name;
            UserId = userId;
            Origin = origin;
            Resource = resource;
            State = state;
            Code = code;
            Message = message;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}