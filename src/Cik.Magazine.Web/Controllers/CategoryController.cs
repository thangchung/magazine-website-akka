using System;
using System.Collections.Generic;
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
        // private readonly ICategoryQuery _categoryQuery;
        // private readonly ICategoryService _service;
        private readonly IActorRefFactory _actorSystem;

        public CategoryController(
                IActorRefFactory actorSystem)
            // ICategoryService service, 
            // ICategoryQuery categoryQuery)
        {
            _actorSystem = actorSystem;
            // _service = service;
            // _categoryQuery = categoryQuery;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryView>> GetAsync()
        {
            var server = _actorSystem.ActorSelection("akka.tcp://magazine-system@localhost:8092/user/category-service");
            // var result = server.Ask("ping.").Result;
            server.Tell(new CreateCategory(Guid.NewGuid(), "sport"));

            return new List<CategoryView>();
            // return await _categoryQuery.GetCategoryViews(x => true);
        }

        [HttpGet("{id}")]
        public async Task<CategoryView> GetAsync(Guid id)
        {
            // return await _categoryQuery.GetCategoryView(id);
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<object> PostAsync([FromBody] CategoryDto cat)
        {
            // return await _service.Create(cat);
            throw new NotImplementedException();
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