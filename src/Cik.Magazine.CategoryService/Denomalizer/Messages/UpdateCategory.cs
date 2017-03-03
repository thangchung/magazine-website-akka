using System;

namespace Cik.Magazine.CategoryService.Denomalizer.Messages
{
    public class UpdateCategory
    {
        public UpdateCategory(Guid key, string name)
        {
            Key = key;
            Name = name;
        }

        public Guid Key { get; }
        public string Name { get; }
    }
}