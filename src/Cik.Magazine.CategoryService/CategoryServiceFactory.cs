using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Routing;
using Cik.Magazine.CategoryService.Denomalizer.Projections;
using Cik.Magazine.CategoryService.Domain;
using Cik.Magazine.CategoryService.Query;
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
            // var nameOfProcessManagerActor = "category-process-manager";

            // build up the category actor
            var projectionsProps = new ConsistentHashingPool(10)
                .Props(Props.Create<ReadModelProjections>());
            var projections = system.ActorOf(projectionsProps, $"{nameofProjectionActor}-{nameOfCommanderActor}");

            /*var processManagerProps = new ConsistentHashingPool(1)
                .Props(Props.Create(() => new CategoryProcessManager(id)));*/
            // var processManager = system.ActorOf(Props.Create(() => new CategoryProcessManager(id)));
            // var categoryStatusSaga = system.ActorSelection($"/user/{SystemData.CategoryStatusSagaActor.Name}-group");

            var categoryStatusSagaActor = system.ActorOf(
                Props.Empty.WithRouter(FromConfig.Instance), "category-status-broadcaster-group");

            var creationParams = new AggregateRootCreationParameters(id, projections,
                new HashSet<IActorRef>(new List<IActorRef> { categoryStatusSagaActor }), snapshotThreshold);
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