using System;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Domain;

namespace Cik.Magazine.MasterService.Domain
{
    internal class TenantState
    {
        public TenantState(IEventSink events)
        {
            Events = events;
        }

        public string Name { get; set; }
        public AuthenticationInfo AuthInfo { get; set; }
        public DatabaseInfo DbInfo { get; set; }
        internal IEventSink Events { get; set; }

        public void Handle(ICommand command)
        {
            ((dynamic)this).Handle((dynamic)command);
        }

        public void Mutate(IEvent @event)
        {
            ((dynamic)this).Apply((dynamic)@event);
        }

        public override string ToString()
        {
            return string.Join(", ", Events);
        }
    }

    internal class AuthenticationInfo
    {
        public Guid TenantId { get; set; }
        public string TenantName { get; set; }
    }

    internal class DatabaseInfo
    {
        public string Endpoint { get; set; }
    }
}