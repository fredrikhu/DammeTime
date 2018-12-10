using System;
using System.Collections.Generic;
using System.Text;

namespace DammeTime.Core.Domain.Exceptions
{
    public class InvalidWorkdaySpan : Exception
    {
        public InvalidWorkdaySpan()
        {

        }

        public InvalidWorkdaySpan(string message)
            : base(message)
        {
            
        }

        public InvalidWorkdaySpan(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
