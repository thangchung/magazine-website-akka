using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using Akka.Event;
using Cik.Magazine.Core.Views;
using MongoDB.Driver;

namespace Cik.Magazine.CategoryService.Query
{
    public class CategoryQueryActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log;
        // TODO: will refactor later
        private readonly MongoClient _mongoClient = new MongoClient(new MongoUrl("mongodb://127.0.0.1:27017"));

        public CategoryQueryActor()
        {
            _log = Context.GetLogger();

            Receive<ListCategoryViewRequest>(x =>
            {
                _log.Info("Received message[{0}]", x.GetType().Name);
                Sender.Tell(GetCategoryViews(), Self);
            });

            Receive<CategoryViewRequest>(x =>
            {
                _log.Info("Received message[{0}]", x.GetType().Name);
                Sender.Tell(GetCategoryView(x.Id), Self);
            });
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

        private List<CategoryViewResponse> GetCategoryViews()
        {
            _log.Info("Start to query data in NoSQL data source.");
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryViewResponse>("categories");
            var result = col.Find(x => true).ToList();
            _log.Info("Finished querying data.");
            return result;
        }

        private CategoryViewResponse GetCategoryView(Guid id)
        {
            _log.Info("Start to query data in NoSQL data source.");
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryViewResponse>("categories");
            var result = col.Find(x => x.Id == id).FirstOrDefault();
            _log.Info("Finished querying data.");
            return result;
        }
    }
}