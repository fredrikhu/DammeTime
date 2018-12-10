using System;

namespace DammeTime.Core.Domain.Entities
{
    public class OrderRegistration
    {
        private TimeRange _timeRange;

        public Guid Guid { get; }
        public OrderNumber OrderNumber { get; }
        public TimeSpan Start => _timeRange.Start;
        public TimeSpan Stop => _timeRange.Stop;
        public TimeSpan Duration => _timeRange.Duration;

        public OrderRegistration(Guid guid, OrderNumber orderNumber, TimeRange timeRange)
        {
            Guid = guid;
            OrderNumber = orderNumber;
            _timeRange = timeRange;
        }

        public bool Overlaps(OrderRegistration other)
        {
            return _timeRange.Overlaps(other._timeRange);
        }
    }
}