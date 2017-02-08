using System;

namespace Cik.Magazine.Core.Messages.Category
{
    public class CreateCategory : ICommand
    {
        public CreateCategory(Guid aggregateId, string data)
        {
            Name = data;
            AggregateId = aggregateId;
        }

        public string Name { get; }

        public Guid AggregateId { get; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class CategoryCreated : Event
    {
        public CategoryCreated(Guid aggregateId, string name)
            : base(aggregateId)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}