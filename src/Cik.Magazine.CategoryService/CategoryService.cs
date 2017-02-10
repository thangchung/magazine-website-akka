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
            GlobalActorSystem = ActorSystem.Create("magazine-system");
            CategoryServiceActor = GlobalActorSystem.ActorOf<CategoryActor>("category-service");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            CategoryServiceActor.Tell(PoisonPill.Instance);
            GlobalActorSystem.Terminate();
            return true;
        }
    }

    public class CategoryActor : TypedActor, IHandle<string>, IHandle<CreateCategory>, ILogReceive
    {
        public void Handle(CreateCategory message)
        {
            Console.WriteLine("Got the message: " + message.Name);
            Sender.Tell(new CreateCategory(message.AggregateId, message.Name + "-procceed."));
        }

        public void Handle(string message)
        {
            Console.WriteLine("Got the string message: " + message);
            Sender.Tell("pong.");
        }
    }
}