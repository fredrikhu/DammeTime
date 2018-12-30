using System;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events
{
    public class AddTimeRegistrationDomainEvent
    {
        public Guid Id { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan Stop { get; set; }
        public string OrderNumber { get; set; }
        public Guid OrderNumberId { get; set; }

        private AddTimeRegistrationDomainEvent()
        {

        }

        public AddTimeRegistrationDomainEvent(AddTimeRegistrationInputEvent @event)
        {
            Id = @event.Id;
            Start = @event.Start.TimeOfDay;
            Stop = @event.Stop;
            OrderNumber = @event.OrderNumber;
        }
    }
}