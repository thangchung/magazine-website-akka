using System;
using Akka.Routing;

namespace Cik.Magazine.Core
{
    public interface IEvent
    {
        Guid AggregateId { get; }
        DateTime UtcDate { get; }
    }
}