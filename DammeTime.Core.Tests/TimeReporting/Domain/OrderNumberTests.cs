using DammeTime.Core.TimeReporting.Domain;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Domain
{
    public class OrderNumberTests
    {
        protected OrderNumber _orderNumber;

        public OrderNumberTests()
        {
            OrderNumberWithReference("KN213 12345");
        }

        public class A_new_order_number : OrderNumberTests
        {
            [Fact, UnitTest]
            public void contains_provided_reference()
            {
                Assert.Equal("KN213 12345", _orderNumber.Reference);
            }
        }

        public class Any_order_number : OrderNumberTests
        {
            [Fact, UnitTest]
            public void equals_another_order_number_if_references_are_equal()
            {
                var other = new OrderNumber("KN213 12345");

                Assert.Equal(_orderNumber, other);
            }

            [Fact, UnitTest]
            public void does_not_equal_another_order_number_if_references_are_not_equal()
            {
                var other = new OrderNumber("other");

                Assert.NotEqual(_orderNumber, other);
            }
        }

        protected void OrderNumberWithReference(string reference)
        {
            _orderNumber = new OrderNumber(reference);
        }
    }
}
