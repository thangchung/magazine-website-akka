using System;
using Akka.Routing;

namespace Cik.Magazine.Core
{
    public abstract class Event : IEvent, IConsistentHashable
    {
        protected Event(Guid aggregateId)
        {
            AggregateId = aggregateId;
            UtcDate = SystemClock.UtcNow;
        }

        public Guid AggregateId { get; private set; }

        public DateTime UtcDate { get; private set; }

        object IConsistentHashable.ConsistentHashKey => AggregateId;
    }
}