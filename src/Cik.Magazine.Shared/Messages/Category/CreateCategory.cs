using System;

namespace Cik.Magazine.Shared.Messages.Category
{
    [Serializable]
    public class CreateCategory : ICommand
    {
        public CreateCategory(Guid aggregateId, string name, Status status)
        {
            Name = name;
            Status = status;
            AggregateId = aggregateId;
        }

        public string Name { get; }
        public Status Status { get; }
        public Guid AggregateId { get; }

        public override string ToString()
        {
            return $"{Name} is in {Status} status.";
        }
    }

    public class CategoryCreated : Event
    {
        public CategoryCreated(Guid aggregateId, string name, Status status)
            : base(aggregateId)
        {
            Name = name;
            Status = status;
        }

        public string Name { get; private set; }
        public Status Status { get; private set; }
    }
}