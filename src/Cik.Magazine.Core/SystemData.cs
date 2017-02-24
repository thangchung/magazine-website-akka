using System;
using Akka.Actor;

namespace Cik.Magazine.Core
{
    public class SystemData
    {
        public const string SystemName = "magazine-system";
        public static ActorMetaData CategoryStorageActor = new ActorMetaData("storage", $"akka://{SystemName}/user/category-storage");
        public static ActorMetaData CategoryProjectionsActor = new ActorMetaData("projections", $"akka://{SystemName}/user/category-projections");
        public static ActorMetaData CategoryQueryActor = new ActorMetaData("category-query", $"akka://{SystemName}/user/category-query");
        public static ActorMetaData CategoryCommanderActor = new ActorMetaData("category-commander", $"akka://{SystemName}/user/category-commander");

        public class ActorMetaData
        {
            public ActorMetaData(string name, string path)
            {
                Name = name;
                Path = path;
            }

            public string Name { get; private set; }
            public string Path { get; private set; }
        }

        public static ActorPath Select(IActorRef parent, Type actorType)
        {
            return parent.Path.Child(actorType.Name.ToLowerInvariant());
        }
    }
}