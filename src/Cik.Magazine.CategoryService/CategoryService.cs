using System;
using Akka.Actor;
using Cik.Magazine.Shared.Messages.Category;
using MongoDB.Bson.Serialization;
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
            CategoryCommanderActor = GlobalActorSystem.CategoryCommanderAggregate(new Guid("8f88d4f42e3c4a868b4667dfe5f97bea"));
            CategoryQueryActor = GlobalActorSystem.CategoryQueryAggregate();

            // config for mongo
            // TODO: need to scan all the events and map it to BsonClassMap
            BsonClassMap.RegisterClassMap<CategoryCreated>();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Information("Release the actor system, query and commander resources.");
            CategoryCommanderActor.Tell(PoisonPill.Instance);
            CategoryQueryActor.Tell(PoisonPill.Instance);
            GlobalActorSystem.Terminate();
            return true;
        }
    }
}