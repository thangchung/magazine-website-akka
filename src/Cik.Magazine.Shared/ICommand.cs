using System;

namespace Cik.Magazine.Shared
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}