using System;

namespace Cik.Magazine.Core.Storage.Messages
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

    public class NewCategoryCreated
    {
        public NewCategoryCreated(Guid key, string name)
        {
            Key = key;
            Name = name;
        }

        public Guid Key { get; private set; }
        public string Name { get; private set; }
    }
}