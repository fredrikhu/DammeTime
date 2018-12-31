using System;
using System.Linq;
using System.Threading.Tasks;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;
using DammeTime.Core.TimeReporting.Domain;
using DammeTime.Core.TimeReporting.Persistence;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class OrderNumberDomainEventPersister
    {
        private readonly ITimeReportingContext _context;

        public OrderNumberDomainEventPersister(ITimeReportingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Persist(AddTimeRegistrationDomainEvent registration)
        {
            var state = new State(registration);

            AddOrderNumber(state);
            AssociateRegistrationWithOrderNumber(state);

            _context.TimeRegistrationDomainEvents.Add(state.Registration);
            await _context.SaveChangesAsync();
        }

        private void AssociateRegistrationWithOrderNumber(State state)
        {
            state.Registration.OrderNumberId = state.OrderNumber.Id;
        }

        private void AddOrderNumber(State state)
        {
            state.OrderNumber = _context.AddOrderNumberDomainEvents.FirstOrAddDescending(
                x => x.OrderNumber == state.Registration.OrderNumber,
                () => new AddOrderNumberDomainEvent { OrderNumber = state.Registration.OrderNumber },
                x => x.EventOrder
            );
        }

        private class State
        {
            public AddTimeRegistrationDomainEvent Registration { get; }
            public AddOrderNumberDomainEvent OrderNumber { get; set; }

            public State(AddTimeRegistrationDomainEvent registration)
            {
                Registration = registration;
            }
        }
    }
}