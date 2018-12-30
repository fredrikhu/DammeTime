using System;
using System.Threading;
using System.Threading.Tasks;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Validation;
using DammeTime.Core.TimeReporting.Persistence;
using MediatR;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class AddTimeRegistrationHandler : IRequestHandler<AddTimeRegistrationCommand, Unit>
    {
        private readonly ITimeReportingContext _context;
        private readonly RegistrationInputValidation _validation;

        public AddTimeRegistrationHandler(ITimeReportingContext context, RegistrationInputValidation validation)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<Unit> Handle(AddTimeRegistrationCommand command, CancellationToken cancellationToken)
        {
            await Save(command.Event);
            await SaveDomainEvent(command.Event);

            return await Unit.Task;
        }

        private async Task Save(AddTimeRegistrationInputEvent @event)
        {
            _validation.Validate(@event);

            _context.TimeRegistrationEvents.Add(@event);
            await _context.SaveChangesAsync();
        }

        private async Task SaveDomainEvent(AddTimeRegistrationInputEvent input)
        {
            var persister = new OrderNumberDomainEventPersister(_context, new AddTimeRegistrationDomainEvent(input));
            await persister.Persist();
        }
    }
}