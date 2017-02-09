using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Cik.Magazine.Core.Messages.Category;
using Cik.Magazine.Core.Services;
using Cik.Magazine.Core.Storage.Query;
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
            var categoryServiceRef = _actorSystem.ActorOf(Props.Create<CategorySystemActor>());
            categoryServiceRef.Tell(new CreateCategory(Guid.NewGuid(), "sample"));

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

    public class CategorySystemActor : TypedActor, IHandle<CreateCategory>, ILogReceive
    {
        private readonly ActorSelection _server 
            = Context.ActorSelection("akka.tcp://CategorySystem@localhost:8091/user/CategoryService");

        public void Handle(CreateCategory message)
        {
            _server.Tell(message);
        }
    }
}