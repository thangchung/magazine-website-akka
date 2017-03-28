using System;
using System.Collections.Generic;
using Akka;
using Akka.Actor;
using Akka.Cluster;
using Akka.Persistence;
using Cik.Magazine.Shared.Messages;

namespace Cik.Magazine.Shared.Domain
{
    public class AggregateRootCreationParameters
    {
        public AggregateRootCreationParameters(Guid id, IActorRef projections, ISet<IActorRef> processManagers,
            int snapshotThreshold = 250)
        {
            Id = id;
            Projections = projections;
            ProcessManagers = processManagers;
            SnapshotThreshold = snapshotThreshold;
        }

        public Guid Id { get; }
        public IActorRef Projections { get; }
        public ISet<IActorRef> ProcessManagers { get; }
        public int SnapshotThreshold { get; }
    }

    public abstract class AggregateRootActor : PersistentActor, IEventSink
    {
        protected Cluster Cluster = Cluster.Get(Context.System);
        private readonly Guid _id;
        private readonly IActorRef _projections;
        private readonly int _snapshotThreshold;
        protected readonly ISet<IActorRef> ProcessManagers;

        protected AggregateRootActor(AggregateRootCreationParameters parameters)
        {
            _id = parameters.Id;
            _projections = parameters.Projections;
            _snapshotThreshold = parameters.SnapshotThreshold;

            ProcessManagers = parameters.ProcessManagers;
        }

        public override string PersistenceId => $"{GetType().Name}-agg-{_id:n}".ToLowerInvariant();

        private long LastSnapshottedVersion { get; set; }

        void IEventSink.Publish(IEvent @event)
        {
            Persist(@event, e =>
            {
                Apply(e);
                _projections.Tell(@event);
                Self.Tell(SaveAggregate.Message); // save the snapshot if it is possible
            });
        }

        protected override bool ReceiveRecover(object message)
        {
            return message.Match()
                .With<RecoveryCompleted>(x => { Log.Debug("Recovered state to version {0}", LastSequenceNr); })
                .With<SnapshotOffer>(offer =>
                {
                    Log.Debug("State loaded from snapshot");
                    LastSnapshottedVersion = offer.Metadata.SequenceNr;
                    RecoverState(offer.Snapshot);
                })
                .With<IEvent>(x => Apply(x))
                .WasHandled;
        }

        protected override bool ReceiveCommand(object message)
        {
            return message.Match()
                .With<SaveAggregate>(x => Save())
                .With<SaveSnapshotSuccess>(success =>
                {
                    Log.Debug("Saved snapshot");
                    DeleteMessages(success.Metadata.SequenceNr);
                })
                .With<SaveSnapshotFailure>(failure =>
                {
                    // handle snapshot save failure...
                })
                .With<ICommand>(command =>
                {
                    try
                    {
                        var handled = Handle(command);
                        Sender.Tell(new CommandResponse(handled));
                    }
                    catch (DomainException e)
                    {
                        Sender.Tell(e);
                    }
                }).WasHandled;
        }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, typeof(ClusterEvent.MemberUp));
            Log.Info("PM [{0}]: Send from {1}", Cluster.SelfAddress, Sender);
            base.PreStart();
        }

        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
            base.PostStop();
        }

        private bool Save()
        {
            if (LastSequenceNr - LastSnapshottedVersion >= _snapshotThreshold)
                SaveSnapshot(GetState());

            return true;
        }

        protected abstract bool Handle(ICommand command);
        protected abstract bool Apply(IEvent @event);
        protected abstract void RecoverState(object state);
        protected abstract object GetState();
    }
}