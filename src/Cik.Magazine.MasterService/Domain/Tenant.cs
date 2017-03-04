using System;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Domain;

namespace Cik.Magazine.MasterService.Domain
{
    public class Tenant : AggregateRootActor
    {
        public Tenant(AggregateRootCreationParameters parameters)
            : base(parameters)
        {
        }

        protected override bool Handle(ICommand command)
        {
            throw new NotImplementedException();
        }

        protected override bool Apply(IEvent @event)
        {
            throw new NotImplementedException();
        }

        protected override void RecoverState(object state)
        {
            throw new NotImplementedException();
        }

        protected override object GetState()
        {
            throw new NotImplementedException();
        }
    }
}