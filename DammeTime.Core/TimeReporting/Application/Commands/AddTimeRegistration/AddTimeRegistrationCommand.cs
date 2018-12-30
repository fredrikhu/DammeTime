using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;
using MediatR;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    // TODO: Generic command instead? Merge the classes?
    public class AddTimeRegistrationCommand : IRequest
    {
        public AddTimeRegistrationCommand(AddTimeRegistrationInputEvent @event)
        {
            Event = @event;
        }

        public AddTimeRegistrationInputEvent Event { get; }
    }
}