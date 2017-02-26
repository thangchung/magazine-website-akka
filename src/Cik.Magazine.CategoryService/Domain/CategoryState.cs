using Cik.Magazine.Core;
using Cik.Magazine.Core.Domain;
using Cik.Magazine.Core.Messages.Category;

namespace Cik.Magazine.CategoryService.Domain
{
    internal class CategoryState
    {
        public CategoryState(IEventSink events)
        {
            Events = events;
        }

        public string Name { get; set; }
        internal IEventSink Events { get; set; }

        public void Handle(ICommand command)
        {
            ((dynamic) this).Handle((dynamic) command);
        }

        public void Mutate(IEvent @event)
        {
            ((dynamic) this).Apply((dynamic) @event);
        }

        public void Handle(CreateCategory message)
        {
            Events.Publish(new CategoryCreated(message.AggregateId, message.Name));
        }

        public void Handle(UpdateCategory message)
        {
            Events.Publish(new CategoryUpdated(message.AggregateId, message.Name));
        }

        public void Handle(DeleteCategory message)
        {
            Events.Publish(new CategoryDeleted(message.AggregateId));
        }

        public void Apply(CategoryCreated message)
        {
            Name = message.Name;
        }

        public void Apply(CategoryUpdated message)
        {
            Name = message.Name;
        }

        public void Apply(CategoryDeleted message)
        {
        }

        public override string ToString()
        {
            return string.Join(", ", Events);
        }
    }
}