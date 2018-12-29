using DammeTime.Core.TimeReporting.Events;
using MediatR;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    // TODO: Generic command instead?
    public class AddTimeRegistrationCommand : IRequest<Unit>
    {
        public AddTimeRegistrationCommand(AddTimeRegistrationEvent @event)
        {
            Event = @event;
        }

        public AddTimeRegistrationEvent Event { get; }
    }
}