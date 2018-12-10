using DammeTime.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DammeTime.Core.Domain.Entities
{
    public class Day
    {
        private readonly List<OrderRegistration> _orderRegistrations = new List<OrderRegistration>();

        public Date Date { get; }

        public TimeSpan StartOfDay => _orderRegistrations.Any() ?
            _orderRegistrations.Min(x => x.Start) :
            new TimeSpan();
        public TimeSpan EndOfDay => _orderRegistrations.Any() ?
            _orderRegistrations.Max(x => x.Stop) :
            new TimeSpan();
        public IReadOnlyCollection<OrderRegistration> OrderRegistrations => _orderRegistrations
            .OrderBy(x => x.Start)
            .ToList()
            .AsReadOnly();

        public Day(Date date)
        {
            Date = date;
        }
        public void AddOrderRegistration(OrderRegistration registration)
        {
            ValidateTimeOverlap(registration);

            _orderRegistrations.Add(registration);
        }

        private void ValidateTimeOverlap(OrderRegistration registration)
        {
            if (_orderRegistrations.Any(x => x.Overlaps(registration)))
                throw new InvalidOrderRegistration($"{nameof(registration)} is overlapping another registration.");
        }

        public void RemoveOrderRegistration(Guid guid)
        {
            _orderRegistrations.RemoveAll(x => x.Guid == guid);
        }
    }
}
