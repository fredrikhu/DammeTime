using System;

namespace DammeTime.Core.Domain.Services
{
    public interface IWeekDateProvider
    {
        int GetWeekNumberForDate(DateTime date);
        int GetYearForDate(DateTime date);
    }
}