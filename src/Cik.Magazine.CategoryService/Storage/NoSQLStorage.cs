using Akka.Actor;
using Akka.Event;
using Cik.Magazine.CategoryService.Storage.Message;
using Cik.Magazine.Core.Views;
using MongoDB.Driver;

namespace Cik.Magazine.CategoryService.Storage
{
    public class NoSqlStorage : TypedActor, IStorageProvider
    {
        private readonly ILoggingAdapter _log;
        private MongoClient _mongoClient;

        public NoSqlStorage()
        {
            _log = Context.GetLogger();
        }

        public void Handle(CreateNewCategory message)
        {
            // TODO: will refactor later
            _mongoClient = new MongoClient(new MongoUrl("mongodb://127.0.0.1:27017"));
            var db = _mongoClient.GetDatabase("magazine");
            if (db == null)
            {
                db = _mongoClient.GetDatabase("magazine");
            }
            
            var col = db.GetCollection<CategoryViewResponse>("categories");
            if (col == null)
            {
                db.CreateCollection("categories");
                col = db.GetCollection<CategoryViewResponse>("categories");
            }

            col.InsertOne(new CategoryViewResponse
            {
                Name = message.Name
            });
        }
    }
}