using Akka.Actor;
using Cik.Magazine.CategoryService.Storage.Message;

namespace Cik.Magazine.CategoryService.Storage
{
    public interface IStorageProvider : IHandle<CreateNewCategory>
    {
        
    }
}