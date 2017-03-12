using System;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;

namespace Cik.Magazine.CategoryService.Denomalizer.Messages
{
    public class CreateNewCategory
    {
        public CreateNewCategory(Guid id, string name, Status status)
        {
            Id = id;
            Name = name;
            Status = status;
            Created = SystemClock.UtcNow;
        }

        public Guid Id { get; }
        public string Name { get; }
        public Status Status { get; }
        public DateTime Created { get; }
    }
}