using System;
using System.Collections.Generic;
using Coolector.Common.Events.Remarks.Models;

namespace Coolector.Common.Events.Remarks
{
    public class RemarkCreated : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public Guid RemarkId { get; }
        public string UserId { get; }
        public string Username { get; }
        public RemarkCategory Category { get; }
        public IEnumerable<RemarkFile> Photos { get; }
        public RemarkLocation Location { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        protected RemarkCreated()
        {
        }

        public RemarkCreated(Guid requestId, Guid remarkId, 
            string userId, string username,
            RemarkCategory category, RemarkLocation location,
            IEnumerable<RemarkFile> photos, string description,
            DateTime createdAt)
        {
            RequestId = requestId;
            RemarkId = remarkId;
            UserId = userId;
            Username = username;
            Category = category;
            Location = location;
            Photos = photos;
            Description = description;
            CreatedAt = createdAt;
        }

        public class RemarkCategory
        {
            public Guid CategoryId { get; }
            public string Name { get; }

            protected RemarkCategory()
            {
            }

            public RemarkCategory(Guid categoryId, string name)
            {
                CategoryId = categoryId;
                Name = name;
            }
        }

        public class RemarkLocation
        {
            public string Address { get; }
            public double Latitude { get; }
            public double Longitude { get; }

            protected RemarkLocation()
            {
            }

            public RemarkLocation(string address, double latitude, double longitude)
            {
                Address = address;
                Latitude = latitude;
                Longitude = longitude;
            }
        }
    }
}