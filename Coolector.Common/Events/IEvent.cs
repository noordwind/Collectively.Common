using System;

namespace Coolector.Common.Events
{
    public interface IEvent
    {
        Guid RequestId { get; }
    }
}