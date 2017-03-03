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

        public Guid Id { get; }
        public string Name { get; }
        public Guid ParentId { get; }
        public DateTime Created { get; }
    }
}