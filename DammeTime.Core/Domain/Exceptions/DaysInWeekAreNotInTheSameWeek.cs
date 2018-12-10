using System;
using System.Collections.Generic;
using System.Text;

namespace DammeTime.Core.Domain.Exceptions
{
    public class DaysInWeekAreNotInTheSameWeek : Exception
    {
        public DaysInWeekAreNotInTheSameWeek()
            : base()
        {

        }

        public DaysInWeekAreNotInTheSameWeek(string message)
            : base(message)
        {
            
        }

        public DaysInWeekAreNotInTheSameWeek(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
