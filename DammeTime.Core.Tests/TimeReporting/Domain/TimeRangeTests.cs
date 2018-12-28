using System;
using DammeTime.Core.TimeReporting.Domain;
using DammeTime.Core.TimeReporting.Domain.Exceptions;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Domain
{
    public class TimeRangeTests
    {
        protected TimeRange _range;
        private TimeSpan _start;
        private TimeSpan _stop;

        public class A_valid_range : TimeRangeTests
        {
            [Fact]
            public void starts_at_provided_time()
            {
                TimeRangeWith(Time("08:00"), Time("10:00"));

                Assert.Equal(_start, _range.Start);
            }

            [Fact]
            public void stops_at_provided_time()
            {
                TimeRangeWith(Time("08:00"), Time("10:00"));

                Assert.Equal(_stop, _range.Stop);
            }
        }

        public class A_range_that_starts_after_it_ends : TimeRangeTests
        {
            [Fact]
            public void throws_an_exception()
            {
                var ex = Record.Exception(() => TimeRangeWith(Time("10:00"), Time("08:00")));

                Assert.IsType<StopIsNotAfterStart>(ex);
            }
        }

        public class A_range_that_starts_at_the_same_time_it_ends : TimeRangeTests
        {
            [Fact]
            public void throws_an_exception()
            {
                var ex = Record.Exception(() => TimeRangeWith(Time("10:00"), Time("10:00")));

                Assert.IsType<StopIsNotAfterStart>(ex);
            }
        }

        public class Any_range : TimeRangeTests
        {
            [Theory]
            [InlineData("07:00", "11:30", 4.5)]
            [InlineData("10:00", "13:15", 3.25)]
            public void calculates_duration_as_time_between_start_and_stop_in_hours(
                string start,
                string stop,
                double duration)
            {
                TimeRangeWith(Time(start), Time(stop));

                Assert.Equal(duration, _range.Duration);
            }

            [Theory]
            [InlineData("07:00", "11:30")]
            [InlineData("10:00", "13:15")]
            public void equals_another_range_with_the_same_start_and_stop_time(
                string start,
                string stop)
            {
                TimeRangeWith(Time(start), Time(stop));
                var other = new TimeRange(Time(start), Time(stop));

                Assert.Equal(_range, other);
            }

            [Theory]
            [InlineData("06:00", "07:00", "11:30")]
            [InlineData("06:00", "10:00", "13:15")]
            public void does_not_equal_another_range_if_the_start_time_is_different(
                string start1,
                string start2,
                string stop)
            {
                TimeRangeWith(Time(start1), Time(stop));
                var other = new TimeRange(Time(start2), Time(stop));

                Assert.NotEqual(_range, other);
            }

            [Theory]
            [InlineData("07:00", "11:30", "10:00")]
            [InlineData("10:00", "13:15", "11:00")]
            public void does_not_equal_another_range_if_the_stop_time_is_different(
                string start,
                string stop1,
                string stop2)
            {
                var other = new TimeRange(Time(start), Time(stop2));
                TimeRangeWith(Time(start), Time(stop1));

                Assert.NotEqual(_range, other);
            }
        }

        protected void TimeRangeWith(TimeSpan start, TimeSpan stop)
        {
            _start = start;
            _stop = stop;
            _range = new TimeRange(start, stop);
        }

        protected TimeSpan Time(string time)
        {
            return TimeSpan.Parse(time);
        }
    }
}