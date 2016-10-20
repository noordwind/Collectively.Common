using System;

namespace Coolector.Common.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}