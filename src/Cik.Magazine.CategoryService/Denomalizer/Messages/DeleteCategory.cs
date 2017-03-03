using System;

namespace Cik.Magazine.CategoryService.Denomalizer.Messages
{
    public class DeleteCategory
    {
        public DeleteCategory(Guid key)
        {
            Key = key;
        }

        public Guid Key { get; }
    }
}