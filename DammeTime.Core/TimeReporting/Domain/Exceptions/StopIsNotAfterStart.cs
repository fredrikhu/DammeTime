using System;

namespace DammeTime.Core.TimeReporting.Domain.Exceptions
{
    public class StopIsNotAfterStart : Exception
    {
        public StopIsNotAfterStart() : base() { }
        public StopIsNotAfterStart(string message) : base(message) { }
        public StopIsNotAfterStart(string message, Exception ex) : base(message, ex) { }
    }
}