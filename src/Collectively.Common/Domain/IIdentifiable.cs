using System;

namespace Collectively.Common.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}