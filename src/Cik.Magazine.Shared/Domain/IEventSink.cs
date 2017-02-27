namespace Cik.Magazine.Shared.Domain
{
    public interface IEventSink
    {
        void Publish(IEvent @event);
    }
}