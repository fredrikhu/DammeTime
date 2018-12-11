using System;
using DammeTime.Core.TimeReporting.Domain.Exceptions;

namespace DammeTime.Core.TimeReporting.Domain
{
    public class TimeRange : IEquatable<TimeRange>
    {
        public TimeSpan Start { get; }
        public TimeSpan Stop { get; }
        public double Duration => Stop.Subtract(Start).TotalHours;

        public TimeRange(TimeSpan start, TimeSpan stop)
        {
            if (stop <= start)
                throw new StopIsNotAfterStart();

            Start = start;
            Stop = stop;
        }

        public bool Equals(TimeRange other)
        {
            return Start.Equals(other.Start) &&
                Stop.Equals(other.Stop);
        }
    }
}