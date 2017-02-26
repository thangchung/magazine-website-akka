using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Cik.Magazine.Core;
using Cik.Magazine.Core.Messages.Category;
using Cik.Magazine.Core.Views;
using Microsoft.AspNetCore.Mvc;

namespace Cik.Magazine.Web.Controllers
{
    /// <summary>
    /// Category API Controller
    /// </summary>
    // [Authorize]
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ActorSelection _categoryCommander;
        private readonly ActorSelection _categoryQuery;

        /// <summary>
        /// Constructor for CategoryController
        /// </summary>
        /// <param name="actorSystem"></param>
        public CategoryController(IActorRefFactory actorSystem)
        {
            _categoryCommander = actorSystem.ActorSelection($"/user/{SystemData.CategoryCommanderActor.Name}-group");
            _categoryQuery = actorSystem.ActorSelection($"/user/{SystemData.CategoryQueryActor.Name}-group");
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<CategoryViewResponse>> GetAsync()
        {
            var result = await _categoryQuery.Ask<List<CategoryViewResponse>>(
                new ListCategoryViewRequest());
            return result;
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CategoryViewResponse> GetAsync(Guid id)
        {
            var result = await _categoryQuery.Ask<CategoryViewResponse>(
                new CategoryViewRequest(id));
            return result;
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        [HttpPost]
        public bool PostAsync([FromBody] CategoryModel cat)
        {
            _categoryCommander.Tell(new CreateCategory(Guid.NewGuid(), cat.Name, cat.ParentId));
            return true;
        }

        /// <summary>
        /// Edit the category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(Guid id, [FromBody] CategoryModel cat)
        {
            _categoryCommander.Tell(new UpdateCategory(id, cat.Name));
            return true;
        }

        /// <summary>
        ///  Delete the category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            _categoryCommander.Tell(new DeleteCategory(id));
            return true;
        }
    }
}