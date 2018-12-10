using System;
using DammeTime.Core.Domain.Entities;
using Xunit;

namespace DammeTime.Core.Tests.Domain.Entities
{
    public class OrderRegistrationTests
    {
        private OrderRegistration _sut;
        private Guid _guid;
        private OrderNumber _orderNumber;
        private TimeRange _timeRange;

        [Fact]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Sets_Order_Number_To_Provided_Value()
        {
            var reference = "ref";
            var start = "08:00";
            var stop = "10:00";
            OrderRegistrationWith(reference, start, stop);

            var actual = _sut.OrderNumber;

            Assert.Equal(_orderNumber, actual);
        }

        [Fact]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Sets_Guid_To_Provided_Value()
        {
            // Arrange
            var reference = "ref";
            var start = "08:00";
            var stop = "10:00";
            OrderRegistrationWith(reference, start, stop);

            // Act
            var actual = _sut.Guid;

            // Assert
            Assert.Equal(_guid, actual);
        }

        private void OrderRegistrationWith(string reference, string start, string stop)
        {
            _guid = Guid.NewGuid();
            _orderNumber = new OrderNumber(reference);
            _timeRange = new TimeRange(start, stop);
            _sut = new OrderRegistration(_guid, _orderNumber, _timeRange);
        }

        [Theory]
        [InlineData("ref", "08:00", "10:00")]
        [InlineData("ref", "10:00", "10:30")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Sets_Start_And_Stop_To_Provided_Values(string reference, string start, string stop)
        {
            OrderRegistrationWith(reference, start, stop);

            var actualStart = _sut.Start;
            var actualStop = _sut.Stop;

            Assert.Equal(TimeSpan.Parse(start), actualStart);
            Assert.Equal(TimeSpan.Parse(stop), actualStop);
        }

        [Theory]
        [InlineData("ref", "08:00", "10:00", 2)]
        [InlineData("ref", "10:00", "10:30", 0.5)]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Duration_Is_The_Difference_Between_Start_And_Stop(string reference, string start, string stop, double duration)
        {
            OrderRegistrationWith(reference, start, stop);

            var actual = _sut.Duration;

            Assert.Equal(actual.TotalHours, duration);
        }
    }
}
