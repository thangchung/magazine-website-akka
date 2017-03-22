using System;
using System.IO;
using Akka.Actor;
using Akka.Event;
using Cik.Magazine.Shared.Queries;
using MongoDB.Driver;

namespace Cik.Magazine.CategoryService.Query
{
    public class CategoryQuery : TypedActor,
        IHandle<ListCategoryViewRequest>,
        IHandle<CategoryViewRequest>
    {
        private readonly ILoggingAdapter _log;

        // TODO: will refactor later
        private readonly MongoClient _mongoClient = new MongoClient(new MongoUrl("mongodb://127.0.0.1:27017"));

        public CategoryQuery()
        {
            _log = Context.GetLogger();
        }

        public void Handle(CategoryViewRequest message)
        {
            _log.Info("Received message[{0}] and query data in NoSQL data source.", message.GetType().Name);
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryViewResponse>("categories");
            var result = col.Find(x => x.Id == message.Id).FirstOrDefault();
            _log.Info("Finished querying data.");
            Sender.Tell(result, Self);
        }

        public void Handle(ListCategoryViewRequest message)
        {
            // var saga = Context.ActorOf(Props.Create(() => new CategoryProcessManager123(new Guid("8f88d4f42e3c4a868b4667dfe5f97bea"))));
            // saga.Tell()

            /*var saga = Context.ActorOf(Props.Create(() => new CategoryProcessManager()));
            saga.Tell(Cik.Magazine.Shared.Messages.Category.Status.Reviewing);
            saga.Tell(Cik.Magazine.Shared.Messages.Category.Status.Reviewing);
            saga.Tell(Cik.Magazine.Shared.Messages.Category.Status.Reviewing);
            saga.Tell(Cik.Magazine.Shared.Messages.Category.Status.Reviewing);
            saga.Tell(Cik.Magazine.Shared.Messages.Category.Status.Reviewing);*/

            _log.Info("Received message[{0}] and query data in NoSQL data source.", message.GetType().Name);
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryViewResponse>("categories");
            var result = col.Find(x => true).ToList();
            _log.Info("Finished querying data.");
            Sender.Tell(result, Self);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(10, TimeSpan.FromSeconds(30), new LocalOnlyDecider(
                e =>
                {
                    _log.Info("{0}", e.GetType().Name);

                    if (e is IOException || e.InnerException is IOException)
                        return Directive.Restart;

                    return Directive.Stop;
                }));
        }
    }
}