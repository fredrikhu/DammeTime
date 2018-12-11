using DammeTime.Core.TimeReporting.Domain;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Domain
{
    public class TimeRegistrationTests
    {
        protected TimeRegistration _registration;
        protected OrderNumber _orderNumber;

        public class An_new_registration : TimeRegistrationTests
        {
            [Fact]
            public void contains_provided_order_number()
            {
                RegistrationWith(OrderNumber("KN213 12345"));

                Assert.Same(_orderNumber, _registration.OrderNumber);
            }
        }

        protected void RegistrationWith(OrderNumber orderNumber)
        {
            _orderNumber = orderNumber;
            _registration = new TimeRegistration(_orderNumber);
        }

        protected OrderNumber OrderNumber(string reference)
        {
            return new OrderNumber(reference);
        }
    }
}