using Akka.Actor;
using Akka.Event;
using Cik.Magazine.CategoryService.Storage.Message;
using Cik.Magazine.Core;

namespace Cik.Magazine.CategoryService.Storage.Projection
{
    public class CategoryUpdated : TypedActor, IHandle<Core.Messages.Category.CategoryUpdated>
    {
        private readonly ILoggingAdapter _log;
        private readonly IActorRef _storage;

        public CategoryUpdated()
        {
            _storage = Context.ActorOf(Props.Create<NoSqlStorage>(), SystemData.CategoryStorageActor.Name);
            _log = Context.GetLogger();
        }

        public void Handle(Core.Messages.Category.CategoryUpdated message)
        {
            _log.Info("CategoryUpdated is handled.");
            _storage.Tell(new UpdateCategory(message.AggregateId, message.Name));
        }
    }
}