using Akka.Actor;
using Akka.Event;
using Cik.Magazine.CategoryService.Storage.Message;
using Cik.Magazine.Core;

namespace Cik.Magazine.CategoryService.Storage.Projection
{
    public class CategoryCreated : TypedActor, IHandle<Core.Messages.Category.CategoryCreated>
    {
        private readonly IActorRef _storage;
        private readonly ILoggingAdapter _log;

        public CategoryCreated()
        {
            _storage = Context.ActorOf(Props.Create<NoSqlStorage>(), SystemData.StorageActor.Name);
            _log = Context.GetLogger();
        }

        public void Handle(Core.Messages.Category.CategoryCreated message)
        {
            _log.Info("CategoryCreated is handled.");
            _storage.Tell(new CreateNewCategory(message.AggregateId, message.Name));
        }
    }
}