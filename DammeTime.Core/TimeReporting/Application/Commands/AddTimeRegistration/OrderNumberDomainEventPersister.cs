using System;
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

            ValidateDomainEvent(state);
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
            state.OrderNumber = _context.AddOrderNumberDomainEvents.SingleOrAdd(
                x => x.OrderNumber == state.Registration.OrderNumber,
                () => new AddOrderNumberDomainEvent { OrderNumber = state.Registration.OrderNumber }
            );
        }

        // TODO: Not like this, can only catch one validation exception which is not good enough for an UI.
        // TODO: Also only protects invariants for this domain. How to solve?
        // TODO: Also how to protect from several registrations overlapping?
        private void ValidateDomainEvent(State state)
        {
            var timeRegistration = new TimeRegistration(new OrderNumber(state.Registration.OrderNumber), new TimeRange(state.Registration.Start, state.Registration.Stop));
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