using System;

namespace Cik.Magazine.Shared.Messages.Category
{
    [Serializable]
    public class DeleteCategory : ICommand
    {
        public DeleteCategory(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }
    }

    [Serializable]
    public class CategoryDeleted : Event
    {
        public CategoryDeleted(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}