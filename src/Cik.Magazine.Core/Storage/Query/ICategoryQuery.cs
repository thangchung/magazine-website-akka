using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cik.Magazine.Core.Views;

namespace Cik.Magazine.Core.Storage.Query
{
    public interface ICategoryQuery
    {
        Task<IEnumerable<CategoryView>> GetCategoryViews(Expression<Func<CategoryView, bool>> func);
        Task<CategoryView> GetCategoryView(Guid id);
    }
}