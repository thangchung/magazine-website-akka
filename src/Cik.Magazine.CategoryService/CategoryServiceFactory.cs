using System;
using Akka.Actor;
using Akka.Routing;
using Cik.Magazine.CategoryService.Domain;
using Cik.Magazine.CategoryService.Query;
using Cik.Magazine.CategoryService.Storage.Projection;
using Cik.Magazine.Core;
using Cik.Magazine.Core.Domain;

namespace Cik.Magazine.CategoryService
{
    public static class CategoryServiceFactory
    {
        public static IActorRef CategoryCommanderAggregate(this IActorRefFactory system, Guid id, int snapshotThreshold = 250)
        {
            return CategoryAggregate(system, "category-commander", id, snapshotThreshold);
        }

        public static IActorRef CategoryQueryAggregate(this IActorRefFactory system)
        {
            return CategoryQueryAggregate(system, "category-query");
        }

        private static IActorRef CategoryAggregate(IActorRefFactory system, string nameOfActor, Guid id, int snapshotThreshold = 250)
        {
            var projectionsProps = new ConsistentHashingPool(5).Props(Props.Create<ReadModelProjections>());
            var projections = system.ActorOf(projectionsProps, SystemData.ProjectionsActor.Name + $"-{nameOfActor}");
            var creationParams = new AggregateRootCreationParameters(id, projections, snapshotThreshold);
            return system.ActorOf(Props.Create<Category>(creationParams), nameOfActor);
        }

        private static IActorRef CategoryQueryAggregate(IActorRefFactory system, string nameOfActor)
        {
            return system.ActorOf(Props.Create<CategoryQueryActor>(), nameOfActor);
        }
    }
}