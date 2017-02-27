using System;

namespace Cik.Magazine.Shared.Messages.Category
{
    [Serializable]
    public class CreateCategory : ICommand
    {
        public CreateCategory(Guid aggregateId, string name, Guid parentId)
        {
            Name = name;
            ParentId = parentId;
            AggregateId = aggregateId;
        }

        public string Name { get; }
        public Guid ParentId { get; }
        public Guid AggregateId { get; }

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class CategoryCreated : Event
    {
        public CategoryCreated(Guid aggregateId, string name, Guid parentId)
            : base(aggregateId)
        {
            Name = name;
            ParentId = parentId;
        }

        public string Name { get; }
        public Guid ParentId { get; }
    }
}