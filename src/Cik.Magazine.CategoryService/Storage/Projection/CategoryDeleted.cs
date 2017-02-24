using Akka.Actor;
using Akka.Event;
using Cik.Magazine.CategoryService.Storage.Message;
using Cik.Magazine.Core;

namespace Cik.Magazine.CategoryService.Storage.Projection
{
    public class CategoryDeleted : TypedActor, IHandle<Core.Messages.Category.CategoryDeleted>
    {
        private readonly ILoggingAdapter _log;
        private readonly IActorRef _storage;

        public CategoryDeleted()
        {
            _storage = Context.ActorOf(Props.Create<NoSqlStorage>(), SystemData.CategoryStorageActor.Name);
            _log = Context.GetLogger();
        }

        public void Handle(Core.Messages.Category.CategoryDeleted message)
        {
            _log.Info("CategoryDeleted is handled.");
            _storage.Tell(new DeleteCategory(message.AggregateId));
        }
    }
}