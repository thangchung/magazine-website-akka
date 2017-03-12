using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Domain;
using Cik.Magazine.Shared.Messages.Category;

namespace Cik.Magazine.CategoryService.Domain
{
    public class CategoryState
    {
        public string Name { get; private set; }
        public Status Status { get; private set; }
        internal IEventSink EventSink { get; set; }

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
            EventSink.Publish(new CategoryCreated(message.AggregateId, message.Name, message.Status));
        }

        public void Handle(UpdateCategory message)
        {
            EventSink.Publish(new CategoryUpdated(message.AggregateId, message.Name));
        }

        public void Handle(DeleteCategory message)
        {
            EventSink.Publish(new CategoryDeleted(message.AggregateId));
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

        public void Apply(CategoryStatusUpdated message)
        {
            Status = message.Status;
        }

        public override string ToString()
        {
            return string.Join(", ", Name);
        }
    }
}