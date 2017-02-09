using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cik.Magazine.Core.Views;
using MongoDB.Driver;

namespace Cik.Magazine.Core.Storage.Query
{
    public class CategoryQuery : ICategoryQuery
    {
        // TODO: will refactor later
        private readonly MongoClient _mongoClient = new MongoClient(new MongoUrl("mongodb://127.0.0.1:27017"));

        public async Task<IEnumerable<CategoryView>> GetCategoryViews(Expression<Func<CategoryView, bool>> func)
        {
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryView>("categories");
            return await col.Find(func).ToListAsync();
        }

        public async Task<CategoryView> GetCategoryView(Guid id)
        {
            var db = _mongoClient.GetDatabase("magazine");
            var col = db.GetCollection<CategoryView>("categories");
            return await col.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}