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
        public DateTime CreatedAt { get; }

        protected OperationCreated()
        {
        }

        public OperationCreated(Guid requestId, string name,
            string userId, string origin, string resource,
            string state, DateTime createdAt)
        {
            RequestId = requestId;
            Name = name;
            UserId = userId;
            Origin = origin;
            Resource = resource;
            State = state;
            CreatedAt = createdAt;
        }
    }
}