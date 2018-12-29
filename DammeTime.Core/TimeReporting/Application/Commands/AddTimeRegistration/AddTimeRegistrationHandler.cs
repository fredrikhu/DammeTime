using System;
using System.Threading;
using System.Threading.Tasks;
using DammeTime.Core.TimeReporting.Events;
using DammeTime.Core.TimeReporting.Persistence;
using MediatR;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class AddTimeRegistrationHandler : IRequestHandler<AddTimeRegistrationCommand, Unit>
    {
        private readonly ITimeReportingContext _context;

        public AddTimeRegistrationHandler(ITimeReportingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Unit> Handle(AddTimeRegistrationCommand command, CancellationToken cancellationToken)
        {
            await Save(command.Event);
            await SaveDomainEvent(command.Event);

            return await Unit.Task;
        }

        private async Task Save(AddTimeRegistrationEvent @event)
        {
            _context.TimeRegistrationEvents.Add(@event);
            await _context.SaveChangesAsync();
        }

        private async Task SaveDomainEvent(AddTimeRegistrationEvent input)
        {
            var persister = new OrderNumberDomainEventPersister(_context, new AddTimeRegistrationDomainEvent(input));
            await persister.Persist();
        }
    }
}