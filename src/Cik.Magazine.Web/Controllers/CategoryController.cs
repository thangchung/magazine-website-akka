using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cik.Magazine.Core.Services;
using Cik.Magazine.Core.Storage.Query;
using Cik.Magazine.Core.Views;
using Microsoft.AspNetCore.Mvc;

namespace Cik.Magazine.Web.Controllers
{
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryQuery _categoryQuery;
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service, ICategoryQuery categoryQuery)
        {
            _service = service;
            _categoryQuery = categoryQuery;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryView>> GetAsync()
        {
            return await _categoryQuery.GetCategoryViews(x => true);
        }

        [HttpGet("{id}")]
        public async Task<CategoryView> GetAsync(Guid id)
        {
            return await _categoryQuery.GetCategoryView(id);
        }

        [HttpPost]
        public async Task<object> PostAsync([FromBody] CategoryDto cat)
        {
            return await _service.Create(cat);
        }

        [HttpPut("{id}")]
        public Task<bool> Put(int id, [FromBody] CategoryDto cat)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}