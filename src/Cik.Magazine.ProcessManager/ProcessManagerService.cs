using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence.Fsm;
using Akka.Routing;
using Cik.Magazine.ProcessManager.Workflows;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;
using MongoDB.Bson.Serialization;
using Serilog;
using Topshelf;
using Status = Cik.Magazine.Shared.Messages.Category.Status;

namespace Cik.Magazine.ProcessManager
{
    public class ProcessManagerService : ServiceControl
    {
        private readonly ILogger _logger;

        public ProcessManagerService(ILogger logger)
        {
            _logger = logger;
        }

        private ActorSystem GlobalActorSystem { get; set; }
        private IActorRef CategoryStatusSagaActor { get; set; }

        public bool Start(HostControl hostControl)
        {
            GlobalActorSystem = ActorSystem.Create("magazine-system");

            var categorySagaProps =
                new ConsistentHashingPool(2).Props(
                    Props.Create(() => new CategoryStatusWorkflow(
                        new Guid("8f88d4f42e3c4a868b4667dfe5f97bea"))));

            CategoryStatusSagaActor = GlobalActorSystem.ActorOf(
                categorySagaProps,
                "category-status-broadcaster");

            if (!BsonClassMap.IsClassMapRegistered(typeof(PersistentFSMBase<,,>.StateChangeEvent)))
                BsonClassMap.RegisterClassMap<PersistentFSMBase<Status, List<CategoryData>, Event>.StateChangeEvent>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(CategoryStatusUpdated)))
                BsonClassMap.RegisterClassMap<CategoryStatusUpdated>();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            StopAsync().Wait();
            return true;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/39656932/how-to-handle-async-start-errors-in-topshelf
        /// </summary>
        /// <returns></returns>
        private async Task StopAsync()
        {
            CategoryStatusSagaActor.Tell(PoisonPill.Instance);
            await GlobalActorSystem.Terminate();
        }
    }
}