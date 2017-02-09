using System;
using Akka.Actor;
using Cik.Magazine.Core.Messages.Category;
using Topshelf;

namespace Cik.Magazine.CategoryService
{
    public class CategoryService : ServiceControl
    {
        protected ActorSystem GlobalActorSystem { get; set; }
        protected IActorRef CategoryServiceActor { get; set; }

        public bool Start(HostControl hostControl)
        {
            GlobalActorSystem = ActorSystem.Create("CategorySystem");
            CategoryServiceActor = GlobalActorSystem.ActorOf<SampleActor>("CategoryService");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            CategoryServiceActor.Tell(PoisonPill.Instance);
            GlobalActorSystem.Terminate();
            return true;
        }
    }

    public class SampleActor : TypedActor, IHandle<CreateCategory>, ILogReceive
    {
        public void Handle(CreateCategory message)
        {
            Console.WriteLine("Got the message: " + message.Name);
        }
    }
}