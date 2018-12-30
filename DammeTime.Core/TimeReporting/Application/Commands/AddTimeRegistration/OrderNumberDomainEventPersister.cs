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
        private readonly AddTimeRegistrationDomainEvent _registration;
        private AddOrderNumberDomainEvent _orderNumber;

        public OrderNumberDomainEventPersister(ITimeReportingContext context, AddTimeRegistrationDomainEvent registration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _registration = registration ?? throw new ArgumentNullException(nameof(registration));
        }

        public async Task Persist()
        {
            ValidateDomainEvent();
            AddOrderNumber();
            AssociateRegistrationWithOrderNumber();

            _context.TimeRegistrationDomainEvents.Add(_registration);
            await _context.SaveChangesAsync();
        }

        private void AssociateRegistrationWithOrderNumber()
        {
            _registration.OrderNumberId = _orderNumber.Id;
        }

        private void AddOrderNumber()
        {
            _orderNumber = _context.AddOrderNumberDomainEvents.SingleOrAdd(
                x => x.OrderNumber == _registration.OrderNumber,
                () => new AddOrderNumberDomainEvent { OrderNumber = _registration.OrderNumber }
            );
        }

        // TODO: Not like this, can only catch one validation exception which is not good enough for an UI.
        // TODO: Also only protects invariants for this domain. How to solve?
        // TODO: Also how to protect from several registrations overlapping?
        private void ValidateDomainEvent()
        {
            var timeRegistration = new TimeRegistration(new OrderNumber(_registration.OrderNumber), new TimeRange(_registration.Start, _registration.Stop));
        }
    }
}