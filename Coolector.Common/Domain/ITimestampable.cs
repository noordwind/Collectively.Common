using System;

namespace Coolector.Common.Domain
{
    public interface ITimestampable
    {
        DateTime CreatedAt { get; }
    }
}