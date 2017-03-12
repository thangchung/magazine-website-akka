using System;
using Akka.Persistence.Fsm;
using Cik.Magazine.CategoryService.Domain;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;

namespace Cik.Magazine.CategoryService.Sagas
{
    public class ReviewCategorySaga : PersistentFSM<Status, CategoryState, Event>
    {
        private readonly Guid _id;

        public ReviewCategorySaga(Guid id)
        {
            _id = id;
        }

        public override string PersistenceId => $"{GetType().Name}-agg-{_id:n}".ToLowerInvariant();

        protected override bool ReceiveRecover(object message)
        {
            return base.ReceiveRecover(message);
        }

        protected override void OnRecoveryCompleted()
        {
            When(Status.Reviewing, (e, state) =>
            {
                if (e.FsmEvent is CategoryCreated)
                {
                    // TODO: do the actions like send email to notify or something else
                    var oldEvent = (CategoryCreated) e.FsmEvent;
                    return
                        state.Applying(new CategoryStatusUpdated(oldEvent.AggregateId, Status.Published));
                }

                return state;
            });
        }

        protected override CategoryState ApplyEvent(Event e, CategoryState data)
        {
            if (e is CategoryStatusUpdated)
            {
                data.Apply((CategoryStatusUpdated) e);
                return data;
            }
            return data;
        }
    }
}