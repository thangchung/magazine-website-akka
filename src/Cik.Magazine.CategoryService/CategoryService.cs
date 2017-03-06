using System;
using Akka.Actor;
using Serilog;
using Topshelf;

namespace Cik.Magazine.CategoryService
{
    public class CategoryService : ServiceControl
    {
        private readonly ILogger _logger;

        public CategoryService(ILogger logger)
        {
            _logger = logger;
        } 

        protected ActorSystem GlobalActorSystem { get; set; }
        protected IActorRef CategoryCommanderActor { get; set; }
        protected IActorRef CategoryQueryActor { get; set; }

        public bool Start(HostControl hostControl)
        {
            _logger.Information("Create an actor system, query and commander.");
            GlobalActorSystem = ActorSystem.Create("magazine-system");
            CategoryCommanderActor = GlobalActorSystem.CategoryCommanderAggregate(Guid.NewGuid());
            CategoryQueryActor = GlobalActorSystem.CategoryQueryAggregate();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Information("Release the actor system, query and commander resources.");
            CategoryCommanderActor.Tell(PoisonPill.Instance);
            CategoryQueryActor.Tell(PoisonPill.Instance);
            GlobalActorSystem.Terminate().Wait(5000);
            return true;
        }
    }
}