using System;

namespace Coolector.Common.Events.Remarks
{
    public class CreateRemarkRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public Guid RemarkId { get; }
        public string UserId { get; }
        public string Code { get; }
        public string Reason { get; }

        public CreateRemarkRejected(Guid requestId, 
            Guid remarkId, string userId, 
            string code, string reason)
        {
            RequestId = requestId;
            RemarkId = remarkId;
            UserId = userId;
            Code = code;
            Reason = reason;
        }
    }
}