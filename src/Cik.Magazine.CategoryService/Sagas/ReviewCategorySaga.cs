using System;
using Akka.Persistence.Fsm;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Messages.Category;

namespace Cik.Magazine.CategoryService.Sagas
{
    public interface IReviewEvent
    {
    }

    public class RemindAdminReview : IReviewEvent
    {
    }

    public class ReviewCategorySaga : PersistentFSM<Status, Event, IReviewEvent>
    {
        private readonly Guid _id;

        public ReviewCategorySaga(Guid id)
        {
            _id = id;
        }

        public override string PersistenceId => $"{GetType().Name}-agg-{_id:n}".ToLowerInvariant();

        protected override void OnRecoveryCompleted()
        {
            throw new NotImplementedException();
        }

        protected override Event ApplyEvent(IReviewEvent e, Event data)
        {
            throw new NotImplementedException();
        }
    }
}