using System;
using Akka.Actor;

namespace Cik.Magazine.SampleService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var globalActorSystem = ActorSystem.Create("magazine-system"))
            {
                var sampleServiceActor = globalActorSystem.ActorOf<SampleActor>("SampleService");
                var catRemote =
                    globalActorSystem.ActorSelection("akka.tcp://magazine-system@localhost:8091/user/CategoryService");
                Console.WriteLine("Send message to CategoryService...");
                catRemote.Tell("hey, cat!");

                Console.ReadKey();
            }
        }
    }

    public class SampleActor : TypedActor, IHandle<string>, ILogReceive
    {
        public void Handle(string message)
        {
            Console.WriteLine("Got the string message: " + message);
        }
    }
}