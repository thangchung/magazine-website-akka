using System;

namespace Cik.Magazine.Shared.Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}