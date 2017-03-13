using System;
using Akka;
using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using Akka.Persistence.Fsm;
using Cik.Magazine.CategoryService.Domain;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;
using Status = Cik.Magazine.Shared.Messages.Category.Status;

namespace Cik.Magazine.CategoryService.Sagas
{
    public class CategoryProcessManager : PersistentFSM<Status, CategoryState, Event>
    {
        private readonly Guid _id;
        private readonly IActorRef _commander;
        private readonly CategoryState _state = new CategoryState();
        private readonly ILoggingAdapter _log;
        private long LastSnapshottedVersion { get; set; }

        public CategoryProcessManager(Guid id, IActorRef commander)
        {
            _id = id;
            _commander = commander;
            _log = Context.GetLogger();

            StartWith(Status.Reviewing, _state);
            When(Status.Reviewing, (e, state) =>
            {
                if (e.FsmEvent is CategoryCreated)
                {
                    var oldEvent = (CategoryCreated)e.FsmEvent;
                    return GoTo(Status.Published)
                        .Applying(new CategoryStatusUpdated(oldEvent.AggregateId, Status.Published));
                }

                return state;
            });
            When(Status.Published, (e, state) =>
            {
                // TODO: do the actions like send email to notify or something else
                return state;
            });
        }

        public override string PersistenceId => $"category-process-manager-{_id}";

        protected override bool ReceiveCommand(object message)
        {
            return message.Match()
                .With<IEvent>(@event =>
                {
                    Persist(@event, e => { });
                }).WasHandled;
        }

        protected override bool ReceiveRecover(object message)
        {
            return message.Match()
                .With<CategoryCreated>(@event =>
                {
                    _state.Apply(@event);
                })
                .With<RecoveryCompleted>(() =>
                {
                    _log.Debug("[PM] Recovered state to version {0}", LastSequenceNr);
                })
                .With<SnapshotOffer>(offer =>
                {
                    LastSnapshottedVersion = offer.Metadata.SequenceNr;
                }).WasHandled;
        }

        protected override CategoryState ApplyEvent(Event e, CategoryState data)
        {
            if (e is CategoryStatusUpdated)
            {
                return data;
            }
            return data;
        }

        protected override void OnRecoveryCompleted()
        {
            
        }
    }
}