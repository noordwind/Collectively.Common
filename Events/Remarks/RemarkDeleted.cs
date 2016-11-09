using System;

namespace Coolector.Common.Events.Remarks
{
    public class RemarkDeleted : IEvent
    {
        public Guid RequestId { get; }
        public Guid Id { get; set; }

        public RemarkDeleted(Guid requestId, Guid id)
        {
            RequestId = requestId;
            Id = id;
        }
    }
}