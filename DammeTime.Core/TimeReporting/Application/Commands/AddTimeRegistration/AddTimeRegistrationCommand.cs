using MediatR;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class AddTimeRegistrationCommand
    {
        public AddTimeRegistrationCommand(AddTimeRegistrationEvent addTimeRegistrationEvent)
        {
            Event = addTimeRegistrationEvent;
        }

        public AddTimeRegistrationEvent Event { get; }
    }
}