using System;
using System.Collections.Generic;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events
{
    public class AddTimeRegistrationInputEvent
    {
        public Guid Id { get; set; }
        public int EventOrder { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan Stop { get; set; }
        public string OrderNumber { get; set; }
    }
}