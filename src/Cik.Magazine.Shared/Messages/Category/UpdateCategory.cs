using System;

namespace Cik.Magazine.Shared.Messages.Category
{
    [Serializable]
    public class UpdateCategory : ICommand
    {
        public UpdateCategory(Guid aggregateId, string name)
        {
            Name = name;
            AggregateId = aggregateId;
        }

        public string Name { get; }
        public Guid AggregateId { get; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class CategoryUpdated : Event
    {
        public CategoryUpdated(Guid aggregateId, string name)
            : base(aggregateId)
        {
            Name = name;
        }

        public string Name { get; }
    }
}