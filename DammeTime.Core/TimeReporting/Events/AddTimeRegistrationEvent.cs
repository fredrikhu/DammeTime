using System;

namespace DammeTime.Core.TimeReporting.Events
{
    public class AddTimeRegistrationEvent
    {
        public Guid Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan Duration { get; set; }
        public string OrderNumber { get; set; }
    }
}