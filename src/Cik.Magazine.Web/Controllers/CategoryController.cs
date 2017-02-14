using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Akka.Actor;
using Cik.Magazine.Core.Messages.Category;
using Cik.Magazine.Core.Views;
using Microsoft.AspNetCore.Mvc;

namespace Cik.Magazine.Web.Controllers
{
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ActorSelection _categoryCommander;
        private readonly ActorSelection _categoryQuery;

        public CategoryController(IActorRefFactory actorSystem)
        {
            _categoryCommander = actorSystem.ActorSelection(ConfigurationManager.AppSettings["CategoryCommander"]);
            _categoryQuery = actorSystem.ActorSelection(ConfigurationManager.AppSettings["CategoryQuery"]);
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryViewResponse>> GetAsync()
        {
            var result = await _categoryQuery.Ask<List<CategoryViewResponse>>(
                new ListCategoryViewRequest());
            return result;
        }

        [HttpGet("{id}")]
        public async Task<CategoryViewResponse> GetAsync(Guid id)
        {
            var result = await _categoryQuery.Ask<CategoryViewResponse>(
                new CategoryViewRequest(id));
            return result;
        }

        [HttpPost]
        public bool PostAsync([FromBody] CategoryDto cat)
        {
            _categoryCommander.Tell(new CreateCategory(Guid.NewGuid(), cat.Name));
            return true;
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