using System;
using Akka.Actor;
using Akka.Routing;
using Cik.Magazine.CategoryService.Denomalizer.Projections;
using Cik.Magazine.CategoryService.Domain;
using Cik.Magazine.CategoryService.Query;
using Cik.Magazine.CategoryService.Sagas;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Domain;

namespace Cik.Magazine.CategoryService
{
    public static class CategoryServiceFactory
    {
        public static IActorRef CategoryCommanderAggregate(this IActorRefFactory system, Guid id,
            int snapshotThreshold = 2)
        {
            var nameOfCommanderActor = SystemData.CategoryCommanderActor.Name;
            var nameofProjectionActor = SystemData.CategoryProjectionsActor.Name;
            var nameOfProcessManagerActor = "category-process-manager";

            // build up the category actor
            var projectionsProps = new ConsistentHashingPool(10).Props(Props.Create<ReadModelProjections>());
            var projections = system.ActorOf(projectionsProps, $"{nameofProjectionActor}-{nameOfCommanderActor}");

            // TODO: need to have a way to inject commander into process manager for trigger event back to commander
            var processManagerProps = new ConsistentHashingPool(10).Props(Props.Create(() => new CategoryProcessManager(id, null)));
            var processManager = system.ActorOf(processManagerProps, $"{nameOfProcessManagerActor}");

            var creationParams = new AggregateRootCreationParameters(id, projections, processManager, snapshotThreshold);
            return system.ActorOf(Props.Create<Category>(creationParams), nameOfCommanderActor);
        }

        public static IActorRef CategoryQueryAggregate(this IActorRefFactory system)
        {
            var nameOfQueryActor = SystemData.CategoryQueryActor.Name;
            var queryProps = new ConsistentHashingPool(10).Props(Props.Create<CategoryQuery>());
            return system.ActorOf(queryProps, $"{nameOfQueryActor}");
        }
    }
}