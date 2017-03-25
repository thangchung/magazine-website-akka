using System;
using System.Collections.Generic;
using System.Linq;
using Akka;
using Akka.Event;
using Akka.Persistence;
using Akka.Persistence.Fsm;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;
using MongoDB.Bson.Serialization;

namespace Cik.Magazine.CategoryService.Sagas
{
    public class CategoryData
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }

    /// <summary>
    ///     CategoryProcessManager workflow for this case as
    ///     1. [CategoryAggreation] submits CreateCategory command to [CategoryProcessManager] (status is Draft)
    ///     2. [CategoryProcessManager] sends an email to [Admin] for an approval process (status is WaitingForApproval)
    ///     2.1 This process will run periodic if [Admin] doesn't do the approval process (Quatz)
    ///     3. [Admin] knows about it, then submits the ApproveCategory command to the [CategoryAggreation]
    ///     4. [CategoryAggreation] updates itself status to Published and tells [CategoryProcessManager] about it
    ///     5. [CategoryProcessManager] sends an email to notify with [Admin], and triggers CategoryApproved event for
    ///     persistence it into the storage
    /// </summary>
    public class CategoryProcessManager : PersistentFSM<Status, List<CategoryData>, Event>
    {
        private readonly List<CategoryData> _data = new List<CategoryData>();
        private readonly Guid _id;
        private readonly ILoggingAdapter _log;

        public CategoryProcessManager(Guid id)
        {
            _id = id;
            _log = Context.GetLogger();

            if (!BsonClassMap.IsClassMapRegistered(typeof(StateChangeEvent)))
                BsonClassMap.RegisterClassMap<StateChangeEvent>();

            StartWith(Status.Draft, _data);

            When(Status.Draft, (e, state) =>
            {
                if (e.FsmEvent is CreateCategory)
                {
                    var oldEvent = (CreateCategory) e.FsmEvent;
                    return GoTo(Status.WaitingForApproval)
                        .Applying(new CategoryStatusUpdated(oldEvent.AggregateId, Status.WaitingForApproval));
                }
                return state;
            });

            When(Status.Draft, (e, state) =>
            {
                // TODO: 1. handle e.FsmEvent is ApproveCategory
                // TODO: 2. GoTo(Status.Published)
                // TODO: 3. Applying(new CategoryApproved(oldEvent.AggregateId, Status.Published))

                return state;
            });

            When(Status.Published, (e, state) => { return state; });
        }

        private long LastSnapshottedVersion { get; set; }

        public override string PersistenceId => $"category-process-manager-{_id}";

        protected override bool ReceiveRecover(object message)
        {
            return message.Match()
                .With<CategoryStatusUpdated>(@event =>
                {
                    if (!_data.Any())
                    {
                        _data.Add(new CategoryData
                        {
                            Id = @event.AggregateId,
                            Status = @event.Status
                        });
                    }
                    else
                    {
                        var temps = _data.ConvertAll(x => x);
                        foreach (var categoryData in temps)
                            if (categoryData.Id.Equals(@event.AggregateId))
                                categoryData.Status = @event.Status;
                            else
                                _data.Add(new CategoryData
                                {
                                    Id = @event.AggregateId,
                                    Status = @event.Status
                                });
                    }
                })
                .With<RecoveryCompleted>(() =>
                {
                    _log.Debug("[PM] Recovered state to version {0}", LastSequenceNr);
                    OnRecoveryCompleted();
                })
                .With<SnapshotOffer>(offer => { LastSnapshottedVersion = offer.Metadata.SequenceNr; }).WasHandled;
        }

        protected override List<CategoryData> ApplyEvent(Event e, List<CategoryData> data)
        {
            if (e is CategoryStatusUpdated)
            {
                var evented = (CategoryStatusUpdated) e;
                // TODO: refactor later
                if (!_data.Any())
                {
                    _data.Add(new CategoryData
                    {
                        Id = evented.AggregateId,
                        Status = evented.Status
                    });
                }
                else
                {
                    var temps = _data.ConvertAll(x => x);
                    foreach (var categoryData in temps)
                        if (categoryData.Id.Equals(evented.AggregateId))
                            categoryData.Status = evented.Status;
                        else
                            _data.Add(new CategoryData
                            {
                                Id = evented.AggregateId,
                                Status = evented.Status
                            });
                }

                return data;
            }
            return data;
        }

        protected override void OnRecoveryCompleted()
        {
            foreach (var categoryData in _data)
                if (categoryData.Status == Status.WaitingForApproval)
                {
                    // TODO: [side-effects] send notification to sys-admin for approve the category 
                    // TODO: e.g EmailActor.Tell(SendEmail())
                }
        }
    }
}