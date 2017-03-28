using System;
using Akka.Actor;

namespace Cik.Magazine.Shared.Actors
{
    /// <summary>
    ///     Print all actors in akka system
    ///     http://stackoverflow.com/questions/25860939/how-print-all-actors-in-akka-system
    ///     Usage: _actorSystem.ActorOf(Props.Create<ListOutActor>());
    /// </summary>
    public class ListOutActor : UntypedActor
    {
        public ListOutActor()
        {
            var selection = Context.ActorSelection("/user/*");
            selection.Tell(new Identify(Guid.NewGuid()), Self);
        }

        protected override void OnReceive(object message)
        {
            if (message is ActorIdentity)
            {
                var identity = (ActorIdentity) message;
            }
        }
    }
}