using System;

namespace DammeTime.Core.Domain.Entities
{
    public class Date
    {
        private readonly DateTimeOffset _value;
        public int Year => _value.Year;
        public int Month => _value.Month;
        public int Day => _value.Day;

        public Date(int year, int month, int day)
        {
            _value = new DateTimeOffset(year, month, day, 0, 0, 0, new TimeSpan());
        }
    }
}
