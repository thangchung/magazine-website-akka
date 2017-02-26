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

        public Guid Key { get; private set; }
        public string Name { get; private set; }
    }
}