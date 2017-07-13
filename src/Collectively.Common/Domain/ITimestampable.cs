using System;

namespace Collectively.Common.Domain
{
    public interface ITimestampable
    {
        DateTime CreatedAt { get; }
    }
}