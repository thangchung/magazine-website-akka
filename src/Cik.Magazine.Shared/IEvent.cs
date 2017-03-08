using System;
using Akka.Routing;
using MongoDB.Bson.Serialization.Attributes;

namespace Cik.Magazine.Shared
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

        [BsonId]
        public Guid AggregateId { get; private set; }

        public DateTime UtcDate { get; private set; }

        object IConsistentHashable.ConsistentHashKey => AggregateId;
    }
}