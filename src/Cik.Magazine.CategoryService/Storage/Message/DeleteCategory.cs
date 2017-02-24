using System;

namespace Cik.Magazine.CategoryService.Storage.Message
{
    public class DeleteCategory
    {
        public DeleteCategory(Guid key)
        {
            Key = key;
        }

        public Guid Key { get; private set; }
    }

    public class CategoryDeleted
    {
        public CategoryDeleted(Guid key)
        {
            Key = key;
        }

        public Guid Key { get; private set; }
    }
}