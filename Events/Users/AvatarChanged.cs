using System;

namespace Coolector.Common.Events.Users
{
    public class AvatarChanged : IEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; set; }
        public string PictureUrl { get; set; }

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