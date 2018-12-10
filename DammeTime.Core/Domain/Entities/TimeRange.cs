using System;

namespace DammeTime.Core.Domain.Entities
{
    public class TimeRange
    {
        public TimeSpan Start { get; }
        public TimeSpan Stop { get; }
        public TimeSpan Duration => Stop.Subtract(Start);

        public TimeRange(TimeSpan start, TimeSpan stop)
        {
            if (start >= stop)
                throw new ArgumentException($"Start time ({start}) cannot be after  or equal stop time ({stop}).");

            Start = start;
            Stop = stop;
        }

        public TimeRange(string start, string stop)
            : this(TimeSpan.Parse(start), TimeSpan.Parse(stop))
        {
        }

        public bool Overlaps(TimeRange other)
        {
            if (other.Start >= Start && other.Start < Stop || other.Stop > Start && other.Stop <= Stop)
                return true;
            if (other.Start < Start && other.Stop > Stop || Start < other.Start && Stop > other.Stop)
                return true;

            return false;
        }
    }
}
