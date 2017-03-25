using System;

namespace Cik.Magazine.Shared.Messages.Category
{
    [Serializable]
    public class UpdateCategoryStatus : ICommand
    {
        public UpdateCategoryStatus(Guid aggregateId, Status status)
        {
            Status = status;
            AggregateId = aggregateId;
        }

        public Status Status { get; }
        public Guid AggregateId { get; }
    }

    public class CategoryStatusUpdated : Event
    {
        public CategoryStatusUpdated(Guid aggregateId, Status status)
            : base(aggregateId)
        {
            Status = status;
        }

        public Status Status { get; private set; }
    }
}