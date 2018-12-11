using System;
using System.Collections.Generic;
using DammeTime.Core.TimeReporting.Domain.Exceptions;

namespace DammeTime.Core.TimeReporting.Domain
{
    public class TimeRegistration
    {
        public OrderNumber OrderNumber { get; }

        public TimeRange Range { get; }

        public TimeRegistration(OrderNumber orderNumber, TimeRange range)
        {
            OrderNumber = orderNumber;
            Range = range;
        }
    }
}
