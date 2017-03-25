using System;
using Akka;
using Akka.Event;
using Akka.Persistence;
using Akka.Persistence.Fsm;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;
using Status = Cik.Magazine.Shared.Messages.Category.Status;

namespace Cik.Magazine.CategoryService.Sagas
{
    public class CategoryData
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }    
    }

    public class CategoryProcessManager : PersistentFSM<Status, CategoryData, Event>
    {
        private readonly Guid _id;
        private readonly CategoryData _data = new CategoryData();
        private readonly ILoggingAdapter _log;
        private long LastSnapshottedVersion { get; set; }

        public CategoryProcessManager(Guid id)
        {
            _id = id;
            _log = Context.GetLogger();

            StartWith(Status.Reviewing, _data);
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
                return state;
            });
        }

        public override string PersistenceId => $"category-process-manager-{_id}";

        protected override bool ReceiveCommand(object message)
        {
            return message.Match()
                .With<IEvent>(@event =>
                {
                    Persist(@event, e =>
                    {
                    });
                }).WasHandled;
        }

        protected override bool ReceiveRecover(object message)
        {
            return message.Match()
                .With<CategoryStatusUpdated>(@event =>
                {
                    _data.Id = @event.AggregateId;
                    _data.Status = @event.Status;
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

        protected override CategoryData ApplyEvent(Event e, CategoryData data)
        {
            if (e is CategoryStatusUpdated)
            {
                // TODO: send notification to sys-admin for approve the category 
                return data;
            }
            return data;
        }

        protected override void OnRecoveryCompleted()
        {
            
        }
    }
}