using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DammeTime.Core.Domain.Exceptions;
using DammeTime.Core.Domain.Services;

namespace DammeTime.Core.Domain.Entities
{
    public class Week
    {
        private List<Day> _days;
        private IWeekDateProvider _weekDateProvider;

        public int WeekNumber => GetWeekNumberForDay(_days.First());
        public int Year => GetYearForDay(_days.First());

        public Week(List<Day> days, IWeekDateProvider weekDateProvider)
        {
            _weekDateProvider = weekDateProvider ?? throw new ArgumentNullException(nameof(weekDateProvider));

            if (DaysHaveDifferentWeekNumber(days))
                throw new DaysInWeekAreNotInTheSameWeek();

            _days = days;
        }

        private bool DaysHaveDifferentWeekNumber(List<Day> days)
        {
            return days.Select(GetWeekNumberForDay)
                .Distinct()
                .Count() > 1;
        }

        private int GetWeekNumberForDay(Day day)
        {
            var localDate = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, 0, 0, 0, DateTimeKind.Local);

            return _weekDateProvider.GetWeekNumberForDate(localDate);
        }

        private int GetYearForDay(Day day)
        {
            var localDate = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, 0, 0, 0, DateTimeKind.Local);
            
            return _weekDateProvider.GetYearForDate(localDate);
        }
    }
}