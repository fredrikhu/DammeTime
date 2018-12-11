using System;
using DammeTime.Core.TimeReporting.Domain;
using DammeTime.Core.TimeReporting.Domain.Exceptions;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Domain
{
    public class TimeRegistrationTests
    {
        protected TimeRegistration _registration;

        public class A_new_registration : TimeRegistrationTests
        {
            [Fact]
            public void has_same_order_number_as_provided()
            {
                RegistrationWith(OrderNumber("KN213 12345"), TimeRange());

                Assert.Equal(OrderNumber("KN213 12345"), _registration.OrderNumber);
            }

            [Fact]
            public void has_the_same_range_as_provided()
            {
                RegistrationWith(OrderNumber(), TimeRange("08:00", "10:00"));

                Assert.Equal(TimeRange("08:00", "10:00"), _registration.Range);
            }
        }

        protected void RegistrationWith(OrderNumber orderNumber, TimeRange range)
        {
            _registration = new TimeRegistration(orderNumber, range);
        }

        protected OrderNumber OrderNumber()
        {
            return new OrderNumber("ref");
        }

        protected OrderNumber OrderNumber(string reference)
        {
            return new OrderNumber(reference);
        }

        protected TimeRange TimeRange()
        {
            return new TimeRange(TimeSpan.Parse("08:00"), TimeSpan.Parse("17:00"));
        }

        protected TimeRange TimeRange(string start, string stop)
        {
            return new TimeRange(TimeSpan.Parse(start), TimeSpan.Parse(stop));
        }
    }
}
