using System;
using System.Collections.Generic;
using System.Linq;

namespace Cik.Magazine.Core.Messages
{
    public class CommandResponse
    {
        private readonly bool _success;
        private readonly Exception[] _exceptions;

        public CommandResponse(bool success, IEnumerable<Exception> exceptions = null)
        {
            _success = success;
            _exceptions = exceptions?.ToArray() ?? new Exception[0];
        }

        public bool Success => _success && _exceptions.Length == 0;

        public AggregateException Exception => Success ? null : new AggregateException(_exceptions);
    }
}