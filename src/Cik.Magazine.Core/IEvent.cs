using System;
using Akka.Routing;

namespace Cik.Magazine.Core
{
    public interface IEvent
    {
        Guid AggregateId { get; }
        DateTime UtcDate { get; }
    }

    public abstract class Event : IEvent, IConsistentHashable
    {
        protected Event(Guid aggregateId)
        {
            AggregateId = aggregateId;
            UtcDate = SystemClock.UtcNow;
        }

        public Guid AggregateId { get; }

        public DateTime UtcDate { get; }

        object IConsistentHashable.ConsistentHashKey => AggregateId;
    }
}