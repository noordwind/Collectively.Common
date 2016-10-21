using System;
using Coolector.Common.Events.Remarks.Models;
using System.Collections.Generic;

namespace Coolector.Common.Events.Remarks
{
    public class RemarkResolved : IEvent
    {
        public Guid RemarkId { get; }
        public string UserId { get; }
        public IEnumerable<RemarkFile> Photos { get; }
        public DateTime ResolvedAt { get; }

        protected RemarkResolved() { }

        public RemarkResolved(Guid remarkId, string userId, IEnumerable<RemarkFile> photos, DateTime resolvedAt)
        {
            RemarkId = remarkId;
            UserId = userId;
            Photos = photos;
            ResolvedAt = resolvedAt;
        }
    }
}