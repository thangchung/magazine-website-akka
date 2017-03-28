using System;
using Akka.Routing;

namespace Cik.Magazine.Shared
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }

    public class Command : ICommand, IConsistentHashable
    {
        public Command(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }
        object IConsistentHashable.ConsistentHashKey => AggregateId;
    }
}