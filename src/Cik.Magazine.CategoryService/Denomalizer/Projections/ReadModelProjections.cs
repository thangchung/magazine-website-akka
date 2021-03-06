﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Cik.Magazine.Shared;

namespace Cik.Magazine.CategoryService.Denomalizer.Projections
{
    // TODO: We need to keep this class in the specific assembly to scan all the handlers 
    // TODO: need to find out the way to move it down to Core project.
    public class ReadModelProjections : ReceiveActor
    {
        public static Type[] ProjectionActors;
        private readonly ILoggingAdapter _log;
        private readonly HashSet<ActorPath> _paths = new HashSet<ActorPath>();

        static ReadModelProjections()
        { 
            var root = typeof(ReadModelProjections);
            ProjectionActors = root.Assembly.GetTypes()
                // ReSharper disable once PossibleNullReferenceException
                // ReSharper disable once AssignNullToNotNullAttribute
                .Where(type => type.Namespace.StartsWith(root.Namespace))
                .Where(type => typeof(IInternalActor).IsAssignableFrom(type))
                .ToArray();
        }

        public ReadModelProjections()
        {
            _log = Context.GetLogger();

            Receive<IEvent>(x =>
            {
                var eventType = x.GetType();
                var actorType = ProjectionActors.FirstOrDefault(a => a.Name == eventType.Name);
                if (actorType != null)
                {
                    var path = SystemData.Select(Self, actorType);
                    if (!_paths.Contains(path))
                    {
                        var @ref = Context.ActorOf(Props.Create(actorType), actorType.Name.ToLowerInvariant());
                        _paths.Add(@ref.Path);
                    }
                    Context.ActorSelection(path).Tell(x);
                }

                Context.System.EventStream.Publish(x);
            });
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(10, TimeSpan.FromSeconds(30), new LocalOnlyDecider(
                e =>
                {
                    _log.Info("{0}", e.GetType().Name);

                    if (e is IOException || e.InnerException is IOException)
                        return Directive.Restart;

                    return Directive.Stop;
                }));
        }
    }
}