using System;

namespace Cik.Magazine.Core.Messages.Category
{
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