using System;
using System.Collections.Generic;
using Akka.Actor;
using Cik.Magazine.Core.Views;
using MongoDB.Driver;

namespace Cik.Magazine.CategoryService.Query
{
    public class CategoryQueryActor : ReceiveActor
    {
        // TODO: will refactor later
        private readonly MongoClient _mongoClient = new MongoClient(new MongoUrl("mongodb://127.0.0.1:27017"));

        public CategoryQueryActor()
        {
            Receive<ListCategoryViewRequest>(x =>
            {
                Console.WriteLine($"Received message[{x.GetType().Name}]");
                Sender.Tell(GetCategoryViews(), Self);
            });

            Receive<CategoryViewRequest>(x =>
            {
                Console.WriteLine($"Received message[{x.GetType().Name}]");
                Sender.Tell(GetCategoryView(x.Id), Self);
            });
        }

        private List<CategoryViewResponse> GetCategoryViews()
        {
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryViewResponse>("categories");
            var result = col.Find(x => true).ToList();
            return result;
        }

        private CategoryViewResponse GetCategoryView(Guid id)
        {
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryViewResponse>("categories");
            var result = col.Find(x => x.Id == id).FirstOrDefault();
            return result;
        }
    }
}