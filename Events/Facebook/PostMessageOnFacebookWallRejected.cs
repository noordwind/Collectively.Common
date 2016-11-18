using System;

namespace Coolector.Common.Events.Facebook
{
    public class PostMessageOnFacebookWallRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Reason { get; }
        public string Message { get; }

        protected PostMessageOnFacebookWallRejected()
        {
        }

        public PostMessageOnFacebookWallRejected(Guid requestId, string userId,
            string reason, string message)
        {
            RequestId = requestId;
            UserId = userId;
            Reason = reason;
            Message = message;
        }
    }
}