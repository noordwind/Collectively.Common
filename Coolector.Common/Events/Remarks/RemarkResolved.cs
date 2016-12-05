using System;
using Coolector.Common.Events.Remarks.Models;
using System.Collections.Generic;

namespace Coolector.Common.Events.Remarks
{
    public class RemarkResolved : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public Guid RemarkId { get; }
        public string UserId { get; }
        public string Username { get; }
        public IEnumerable<RemarkFile> Photos { get; }
        public DateTime ResolvedAt { get; }

        protected RemarkResolved()
        {
        }

        public RemarkResolved(Guid requestId, Guid remarkId, 
            string userId, string username,
            IEnumerable<RemarkFile> photos, DateTime resolvedAt)
        {
            RequestId = requestId;
            RemarkId = remarkId;
            UserId = userId;
            Username = username;
            Photos = photos;
            ResolvedAt = resolvedAt;
        }
    }
}