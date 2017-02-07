using System;

namespace Cik.Magazine.Core
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}