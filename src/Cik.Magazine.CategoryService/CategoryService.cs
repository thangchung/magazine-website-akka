using System;
using Akka.Actor;
using Cik.Magazine.CategoryService.Domain;
using Topshelf;

namespace Cik.Magazine.CategoryService
{
    public class CategoryService : ServiceControl
    {
        protected ActorSystem GlobalActorSystem { get; set; }
        protected IActorRef CategoryCommanderActor { get; set; }
        protected IActorRef CategoryQueryActor { get; set; }

        public bool Start(HostControl hostControl)
        {
            GlobalActorSystem = ActorSystem.Create("magazine-system");
            CategoryCommanderActor = GlobalActorSystem.CategoryCommanderAggregate(Guid.NewGuid());
            CategoryQueryActor = GlobalActorSystem.CategoryQueryAggregate();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            CategoryCommanderActor.Tell(PoisonPill.Instance);
            CategoryQueryActor.Tell(PoisonPill.Instance);
            GlobalActorSystem.Terminate();
            return true;
        }
    }
}