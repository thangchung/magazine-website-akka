using Akka;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Domain;

namespace Cik.Magazine.CategoryService.Domain
{
    public class Category : AggregateRootActor
    {
        private CategoryState _state;

        public Category(AggregateRootCreationParameters parameters)
            : base(parameters)
        {
            _state = new CategoryState {EventSink = this};
        }

        protected override bool Handle(ICommand command)
        {
            _state.Handle(command);

            // span out for all the process managers
            foreach (var pm in ProcessManagers)
            {
                pm.Tell(command, Sender);
            }
            return true;
        }

        protected override bool Apply(IEvent @event)
        {
            _state.Mutate(@event);
            return true;
        }

        protected override void RecoverState(object state)
        {
            state.Match()
                .With<CategoryState>(x =>
                {
                    x.EventSink = this;
                    _state = x;
                });
        }

        protected override object GetState()
        {
            return _state;
        }
    }
}