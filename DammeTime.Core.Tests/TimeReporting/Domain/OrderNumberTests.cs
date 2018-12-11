using DammeTime.Core.TimeReporting.Domain;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Domain
{
    public class OrderNumberTests
    {
        protected OrderNumber _orderNumber;

        public class A_new_order_number : OrderNumberTests
        {
            [Fact]
            public void contains_provided_reference()
            {
                OrderNumberWithReference("KN213 12345");

                Assert.Equal("KN213 12345", _orderNumber.Reference);
            }
        }

        protected void OrderNumberWithReference(string reference)
        {
            _orderNumber = new OrderNumber(reference);
        }
    }
}