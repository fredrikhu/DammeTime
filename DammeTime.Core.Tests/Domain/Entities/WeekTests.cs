using System;
using System.Linq;
using System.Collections.Generic;
using DammeTime.Core.Domain.Entities;
using DammeTime.Core.Tests.Culture;
using Xunit;
using DammeTime.Core.Domain.Exceptions;
using DammeTime.Core.Application;

namespace DammeTime.Core.Tests.Domain.Entities
{
    public class WeekTests
    {
        private Week _sut;

        [Theory]
        [MemberData(nameof(DaysWithDifferentWeek))]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Throws_Exception_If_Not_All_Days_Are_From_The_Same_Week_Culture(string culture, List<Day> days)
        {
            using (new TemporaryCulture(culture))
            {
                var ex = Record.Exception(() => WeekWithDays(days));

                Assert.NotNull(ex);
                Assert.IsType<DaysInWeekAreNotInTheSameWeek>(ex);
            }
        }

        public static IEnumerable<object[]> DaysWithDifferentWeek()
        {
            return new List<object[]>
            {
                // Sunday->Saturday
                GenerateWeek("sv-SE", 2018, 12, 2),
                // Saturday->Friday
                GenerateWeek("en-US", 2018, 12, 1)
            };
        }

        [Theory]
        [MemberData(nameof(DaysWithSameWeek))]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Does_Not_Throw_Exception_If_All_Days_Are_From_The_Same_Week_Culture(string culture, List<Day> days)
        {
            using (new TemporaryCulture(culture))
            {
                var ex = Record.Exception(() => WeekWithDays(days));

                Assert.Null(ex);
            }
        }

        public static IEnumerable<object[]> DaysWithSameWeek()
        {
            return new List<object[]>
            {
                // Monday->Sunday
                GenerateWeek("sv-SE", 2018, 12, 3),
                // Sunday->Saturday
                GenerateWeek("en-US", 2018, 12, 2)
            };
        }

        [Theory]
        [MemberData(nameof(DaysWithWeekNumber))]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Returns_Week_Number_For_Provided_Week(string culture, List<Day> days, int weekNumber)
        {
            using (new TemporaryCulture(culture))
            {
                WeekWithDays(days);

                var actual = _sut.WeekNumber;

                Assert.Equal(weekNumber, actual);
            }
        }

        public static IEnumerable<object[]> DaysWithWeekNumber()
        {
            return new List<object[]>
            {
                // Monday->Sunday
                GenerateWeek("sv-SE", 2018, 12, 03).Append(49).ToArray(),
                GenerateWeek("sv-SE", 2018, 12, 10).Append(50).ToArray(),
                GenerateWeek("sv-SE", 2004, 12, 27).Append(53).ToArray(),
                GenerateWeek("sv-SE", 2005, 01, 03).Append(01).ToArray(),
                GenerateWeek("sv-SE", 2006, 12, 25).Append(52).ToArray(),
                GenerateWeek("sv-SE", 2007, 01, 01).Append(01).ToArray(),
                GenerateWeek("sv-SE", 2008, 12, 22).Append(52).ToArray(),
                GenerateWeek("sv-SE", 2008, 12, 29).Append(01).ToArray(),
                // Sunday->Saturday
                GenerateWeek("en-US", 2018, 12, 2).Append(49).ToArray(),
                GenerateWeek("en-US", 2018, 12, 9).Append(50).ToArray()
            };
        }

        [Theory]
        [MemberData(nameof(DaysWithYear))]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Returns_Week_Number_For_Provided_Year(string culture, List<Day> days, int year)
        {
            using (new TemporaryCulture(culture))
            {
                WeekWithDays(days);

                var actual = _sut.Year;

                Assert.Equal(year, actual);
            }
        }

        public static IEnumerable<object[]> DaysWithYear()
        {
            return new List<object[]>
            {
                // Monday->Sunday
                GenerateWeek("sv-SE", 2018, 12, 03).Append(2018).ToArray(),
                GenerateWeek("sv-SE", 2018, 12, 10).Append(2018).ToArray(),
                GenerateWeek("sv-SE", 2004, 12, 27).Append(2004).ToArray(),
                GenerateWeek("sv-SE", 2005, 01, 03).Append(2005).ToArray(),
                GenerateWeek("sv-SE", 2006, 12, 25).Append(2006).ToArray(),
                GenerateWeek("sv-SE", 2007, 01, 01).Append(2007).ToArray(),
                GenerateWeek("sv-SE", 2008, 12, 22).Append(2008).ToArray(),
                GenerateWeek("sv-SE", 2008, 12, 29).Append(2009).ToArray(),
                // Sunday->Saturday
                GenerateWeek("en-US", 2018, 12, 2).Append(2018).ToArray(),
                GenerateWeek("en-US", 2018, 12, 9).Append(2018).ToArray()
            };
        }

        private void WeekWithDays(List<Day> days)
        {
            _sut = new Week(days, new WeekDateProvider());
        }
 
        private static object[] GenerateWeek(string culture, int year, int month, int firstDay)
        {
            return new object[]
            {
                // Saturday->Friday
                culture,
                Enumerable.Range(firstDay, 7)
                    .Select(x => x <= 31 ?
                        new Day(new Date(year, month, x)) :
                        new Day(new Date(year+1, 1, x - 31)))
                    .ToList()
            };
        }

        [Fact]
        public void Throws_Exception_IF_WeekDateProvider_Is_Null()
        {
            var days = (List<Day>) GenerateWeek("sv-SE", 2018, 12, 03)[1];

            var ex = Record.Exception(() => _sut = new Week(days, null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
   }
}