using System;
using Humanizer;

namespace Coolector.Common.Commands
{
    public class Request
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Resource { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedAt { get; set; }

        public static Request From<T>(Request request) => Create<T>(request.Origin, request.Resource, request.Culture);

        public static Request Create<T>(string origin, string resource, string culture) => new Request
        {
            Id = Guid.NewGuid(),
            Name = typeof(T).Name.Humanize(LetterCasing.LowerCase).Underscore(),
            Origin = origin.StartsWith("/") ? origin.Remove(0, 1) : origin,
            Resource = resource,
            Culture = culture,
            CreatedAt = DateTime.UtcNow
        };
    }
}