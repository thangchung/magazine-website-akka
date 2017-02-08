using Akka.Actor;
using Akka.Event;
using Cik.Magazine.Core.Entities;
using Cik.Magazine.Core.Storage.Messages;
using MongoDB.Driver;

namespace Cik.Magazine.Core.Storage
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
            
            var col = db.GetCollection<Category>("categories");
            if (col == null)
            {
                db.CreateCollection("categories");
                col = db.GetCollection<Category>("categories");
            }

            col.InsertOne(new Category
            {
                Name = message.Name
            });
        }
    }
}