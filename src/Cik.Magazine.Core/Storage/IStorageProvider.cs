using Akka.Actor;
using Cik.Magazine.Core.Storage.Messages;

namespace Cik.Magazine.Core.Storage
{
    public interface IStorageProvider : IHandle<CreateNewCategory>
    {
        
    }
}