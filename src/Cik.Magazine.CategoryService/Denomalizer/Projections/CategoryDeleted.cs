using Akka.Actor;
using Akka.Event;
using Cik.Magazine.CategoryService.Denomalizer.Messages;
using Cik.Magazine.Shared;

namespace Cik.Magazine.CategoryService.Denomalizer.Projections
{
    public class CategoryDeleted : TypedActor, IHandle<Shared.Messages.Category.CategoryDeleted>
    {
        private readonly ILoggingAdapter _log;
        private readonly IActorRef _storage;

        public CategoryDeleted()
        {
            _storage = Context.ActorOf(Props.Create<NoSqlStorage>(), SystemData.CategoryStorageActor.Name);
            _log = Context.GetLogger();
        }

        public void Handle(Shared.Messages.Category.CategoryDeleted message)
        {
            _log.Info("CategoryDeleted is handled.");
            _storage.Tell(new DeleteCategory(message.AggregateId));
        }
    }
}