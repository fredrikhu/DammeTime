using System;
using System.Globalization;
using DammeTime.Core.Domain.Services;

namespace DammeTime.Core.Application
{
    public class WeekDateProvider : IWeekDateProvider
    {
        public int GetWeekNumberForDate(DateTime date)
        {
            date = GetNormalizedDate(date);

            var firstDayOfWeek = CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek;
            var isoWeekRule = CalendarWeekRule.FirstFourDayWeek;

            return CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(date, isoWeekRule, firstDayOfWeek);
        }

        public int GetYearForDate(DateTime date)
        {
            date = GetNormalizedDate(date);
            int yearOffset = 0;

            if (GetWeekNumberForDate(date) > 1 && date.Month == 1)
                yearOffset = -1;

            return CultureInfo.CurrentUICulture.Calendar.GetYear(date) + yearOffset;
        }

        private DateTime GetNormalizedDate(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;

            if (dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Wednesday)
                return date.AddDays(3);

            return date;
        }
    }
}