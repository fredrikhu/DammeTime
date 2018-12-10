using System;
using System.Collections.Generic;
using System.Text;

namespace DammeTime.Core.Domain.Exceptions
{
    public class InvalidOrderRegistration : Exception
    {
        public InvalidOrderRegistration()
        {

        }

        public InvalidOrderRegistration(string message)
            : base(message)
        {
            
        }

        public InvalidOrderRegistration(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
