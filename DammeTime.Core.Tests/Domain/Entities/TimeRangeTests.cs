using System;
using DammeTime.Core.Domain.Entities;
using Xunit;

namespace DammeTime.Core.Tests.Domain.Entities
{
    public class TimeRangeTests
    {
        private TimeRange _sut;

        [Fact]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Sets_Start_And_Stop_To_Provided_TimeSpan_Values()
        {
            var providedStart = new TimeSpan(8, 0, 0);
            var providedStop = new TimeSpan(10, 0, 0);
            TimeRangeWithValues(providedStart, providedStop);

            var start = _sut.Start;
            var stop = _sut.Stop;

            Assert.Equal(new TimeSpan(8, 0, 0), start);
            Assert.Equal(new TimeSpan(10, 0, 0), stop);
        }

        [Fact]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Sets_Start_And_Stop_To_Provided_String_Values()
        {
            var providedStart = "08:00";
            var providedStop = "10:00";
            TimeRangeWithValues(providedStart, providedStop);

            var start = _sut.Start;
            var stop = _sut.Stop;

            Assert.Equal(TimeSpan.Parse(providedStart), start);
            Assert.Equal(TimeSpan.Parse(providedStop), stop);
        }

        private void TimeRangeWithValues(TimeSpan start, TimeSpan stop)
        {
            _sut = new TimeRange(start, stop);
        }

        private void TimeRangeWithValues(string start, string stop)
        {
            _sut = new TimeRange(start, stop);
        }

        [Theory]
        [InlineData("08:00", "10:00", 2)]
        [InlineData("08:00", "10:30", 2.5)]
        [InlineData("11:00", "11:15", 0.25)]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Duration_Is_The_Difference_Between_Start_And_Stop(string start, string stop, double expectedHours)
        {
            TimeRangeWithValues(start, stop);

            var actual = _sut.Duration;

            Assert.Equal(expectedHours, actual.TotalHours);
        }

        
        [Theory]
        [InlineData("08:00", "10:00", "09:00", "09:30")]
        [InlineData("09:00", "09:30", "08:00", "10:00")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Overlaps_If_One_Range_Enclose_The_Other_One(string start1, string stop1, string start2, string stop2)
        {
            TimeRangeWithValues(start1, stop1);
            var other = new TimeRange(start2, stop2);

            var actual = _sut.Overlaps(other);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("08:00", "10:00", "09:00", "11:00")]
        [InlineData("09:00", "11:00", "08:00", "10:00")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Overlaps_If_One_Range_Overlaps_The_Other_One(string start1, string stop1, string start2, string stop2)
        {
            TimeRangeWithValues(start1, stop1);
            var other = new TimeRange(start2, stop2);

            var actual = _sut.Overlaps(other);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("08:00", "10:00", "10:00", "11:00")]
        [InlineData("10:00", "11:00", "08:00", "10:00")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Does_Not_Overlap_If_Ranges_Are_Adjacent_To_One_Another(string start1, string stop1, string start2, string stop2)
        {
            TimeRangeWithValues(start1, stop1);
            var other = new TimeRange(start2, stop2);

            var actual = _sut.Overlaps(other);

            Assert.False(actual);
        }
        [Theory]
        [InlineData("08:00", "10:00", "11:00", "12:00")]
        [InlineData("11:00", "12:00", "08:00", "10:00")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Does_Not_Overlap_If_Ranges_Are_Not_Adjacent_To_One_Another(string start1, string stop1, string start2, string stop2)
        {
            TimeRangeWithValues(start1, stop1);
            var other = new TimeRange(start2, stop2);

            var actual = _sut.Overlaps(other);

            Assert.False(actual);
        }

        [Fact]
        public void Throws_Exception_If_Stop_Is_Before_Start()
        {
            var start = "10:00";
            var stop = "09:59";

            var ex = Record.Exception(() => new TimeRange(start, stop));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }
    }
}