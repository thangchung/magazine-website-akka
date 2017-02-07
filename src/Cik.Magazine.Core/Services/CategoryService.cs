using System;
using Akka.Actor;
using Cik.Magazine.Core.Domain;
using Cik.Magazine.Core.Messages;
using Cik.Magazine.Core.Messages.Category;

namespace Cik.Magazine.Core.Services
{
    public class CategoryService
    {
        private readonly IActorRef _actor;

        public CategoryService(IActorRefFactory actorSystem)
        {
            _actor = actorSystem.CategoryAggregate(Guid.NewGuid());
        }

        public void DoSomething()
        {
            _actor.Tell(new CreateCategory(Guid.NewGuid(), "sport"));
            _actor.Tell(SaveAggregate.Message);
        }
    }
}