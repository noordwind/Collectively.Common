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

        public static Request From<T>(Request request)
            => Create<T>(request.Id, request.Origin, request.Culture, request.Resource);

        public static Request Create<T>(Guid id, string origin, string culture, string resource = "")
            => new Request
            {
                Id = id,
                Name = typeof(T).Name.Humanize(LetterCasing.LowerCase).Underscore(),
                Origin = origin.StartsWith("/") ? origin.Remove(0, 1) : origin,
                Culture = culture,
                Resource = resource,
                CreatedAt = DateTime.UtcNow
            };
    }
}