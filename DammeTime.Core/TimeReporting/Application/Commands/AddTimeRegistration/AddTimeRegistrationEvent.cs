using System;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class AddTimeRegistrationEvent
    {
        public Guid Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan Duration { get; set; }
    }
}