using System;

namespace Cik.Magazine.CategoryService.Denomalizer.Messages
{
    public class CreateNewCategory
    {
        public CreateNewCategory(Guid key, string name)
        {
            Key = key;
            Name = name;
        }

        public Guid Key { get; private set; }
        public string Name { get; private set; }
    }
}