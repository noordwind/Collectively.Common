using System;

namespace Coolector.Common.Events.Facebook
{
    public class MessageOnFacebookWallPosted : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Message { get; }

        protected MessageOnFacebookWallPosted()
        {
        }

        public MessageOnFacebookWallPosted(Guid requestId, string userId, string message)
        {
            RequestId = requestId;
            UserId = userId;
            Message = message;
        }
    }
}