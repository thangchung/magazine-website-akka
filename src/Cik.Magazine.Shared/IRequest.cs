using System;
using Akka.Routing;

namespace Cik.Magazine.Shared
{
    public interface IRequest
    {
    }

    public abstract class Request : IRequest, IConsistentHashable
    {
        protected Request()
        {
            RequestUniqueId = Guid.NewGuid();
        }

        public Guid RequestUniqueId { get; }
        object IConsistentHashable.ConsistentHashKey => RequestUniqueId;
    }
}