/* using System;
using System.Threading.Tasks;
using Akka.Actor;
using Cik.Magazine.Core.Views;

namespace Cik.Magazine.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IActorRef _actor;

        public CategoryService(IActorRefFactory actorSystem)
        {
            _actor = actorSystem.CategoryAggregate(Guid.NewGuid());
        }

        public Task<object> Create(CategoryDto cat)
        {
            return _actor.Ask(new CreateCategory(Guid.NewGuid(), cat.Name));
        }
    }
} */