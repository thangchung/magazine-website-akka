using System.Collections.Generic;
using Akka;
using Akka.Actor;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Domain;

namespace Cik.Magazine.CategoryService.Domain
{
    public class Category : AggregateRootActor
    {
        private CategoryState _state;
        public ISet<ActorSelection> ActorSelections { get; }

        public Category(AggregateRootCreationParameters parameters)
            : base(parameters)
        {
            _state = new CategoryState {EventSink = this};

            // register sagas 
            ActorSelections = new HashSet<ActorSelection>
            {
                Context.ActorSelection("/user/category-status-broadcaster-group")
            };
        }

        protected override bool Handle(ICommand command)
        {
            _state.Handle(command);

            // span out to sagas
            foreach (var pm in ActorSelections)
            {
                pm.Tell(command);
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