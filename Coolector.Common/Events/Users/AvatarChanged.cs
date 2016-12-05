using System;

namespace Coolector.Common.Events.Users
{
    public class AvatarChanged : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get;}
        public string PictureUrl { get; }

        protected AvatarChanged()
        {
        }

        public AvatarChanged(Guid requestId, string userId, string pictureUrl)
        {
            RequestId = requestId;
            UserId = userId;
            PictureUrl = pictureUrl;
        }
    }
}