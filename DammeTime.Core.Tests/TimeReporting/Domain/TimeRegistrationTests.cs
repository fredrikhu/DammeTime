using System;
using DammeTime.Core.TimeReporting.Domain;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Domain
{
    public class TimeRegistrationTests
    {
        protected TimeRegistration _registration;
        private OrderNumber _orderNumber;
        private TimeRange _timeRange;

        public TimeRegistrationTests()
        {
            RegistrationWith(OrderNumber("KN213 12345"), TimeRange("08:00", "10:00"));
        }

        public class A_new_registration : TimeRegistrationTests
        {
            [Fact, UnitTest]
            public void has_same_order_number_as_provided()
            {
                Assert.Equal(_orderNumber, _registration.OrderNumber);
            }

            [Fact, UnitTest]
            public void has_the_same_range_as_provided()
            {
                Assert.Equal(_timeRange, _registration.Range);
            }
        }

        protected void RegistrationWith(OrderNumber orderNumber, TimeRange range)
        {
            _registration = new TimeRegistration(orderNumber, range);
        }

        protected OrderNumber OrderNumber(string reference)
        {
            return _orderNumber = new OrderNumber(reference);
        }

        protected TimeRange TimeRange(string start, string stop)
        {
            return _timeRange = new TimeRange(TimeSpan.Parse(start), TimeSpan.Parse(stop));
        }
    }
}
