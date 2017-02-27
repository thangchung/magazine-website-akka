using System;
using Cik.Magazine.Shared;

namespace Cik.Magazine.CategoryService.Denomalizer.Messages
{
    public class CreateNewCategory
    {
        public CreateNewCategory(Guid id, string name, Guid parentId)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            Created = SystemClock.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid ParentId { get; private set; }
        public DateTime Created { get; private set; }
    }
}