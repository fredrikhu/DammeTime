using System;
using System.Collections.Generic;
using DammeTime.Core.Application;
using DammeTime.Core.Tests.Culture;
using Xunit;

namespace DammeTime.Core.Tests.Application
{
    public class WeekDateProviderTests
    {
        private WeekDateProvider _sut;

        [Theory]
        [MemberData(nameof(DaysWithWeekNumber))]
        public void Provides_Correct_Week_Numbers(string culture, DateTime date, int weekNumber)
        {
            using (new TemporaryCulture(culture))
            {
                CreateSut();

                var actual = _sut.GetWeekNumberForDate(date);

                Assert.Equal(weekNumber, actual);
            }
        }

        public static IEnumerable<object[]> DaysWithWeekNumber()
        {
            return new List<object[]>
            {
                GenerateDate("sv-SE", 2018, 12, 03, 49),
                GenerateDate("sv-SE", 2005, 01, 02, 53),
                GenerateDate("sv-SE", 2005, 01, 03, 01),
                GenerateDate("sv-SE", 2006, 12, 31, 52),
                GenerateDate("sv-SE", 2007, 01, 01, 01),
                GenerateDate("sv-SE", 2008, 12, 22, 52),
                GenerateDate("sv-SE", 2008, 12, 29, 01),
                GenerateDate("en-US", 2018, 12, 08, 49),
                GenerateDate("en-US", 2018, 12, 09, 50),
            };
        }

        [Theory]
        [MemberData(nameof(DaysWithYear))]
        public void Provides_Correct_Years(string culture, DateTime date, int year)
        {
            using (new TemporaryCulture(culture))
            {
                CreateSut();

                var actual = _sut.GetYearForDate(date);

                Assert.Equal(year, actual);
            }
        }

        public static IEnumerable<object[]> DaysWithYear()
        {
            return new List<object[]>
            {
                GenerateDate("sv-SE", 2018, 12, 03, 2018),
                GenerateDate("sv-SE", 2005, 01, 02, 2004),
                GenerateDate("sv-SE", 2005, 01, 03, 2005),
                GenerateDate("sv-SE", 2006, 12, 31, 2006),
                GenerateDate("sv-SE", 2007, 01, 01, 2007),
                GenerateDate("sv-SE", 2008, 12, 22, 2008),
                GenerateDate("sv-SE", 2008, 12, 29, 2009),
                GenerateDate("en-US", 2018, 12, 08, 2018),
                GenerateDate("en-US", 2018, 12, 09, 2018),
            };
        }

        public static object[] GenerateDate(string culture, int year, int month, int day, int weekNumberOrYear)
        {
            return new object[]
            {
                culture,
                new DateTime(year, month, day),
                weekNumberOrYear
            };
        }

         private void CreateSut()
         {
             _sut = new WeekDateProvider();
         }
    }
}