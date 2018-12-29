namespace DammeTime.Core.TimeReporting.Domain
{
    // TODO: How to add Date? DateTimeOffset and Duration? Events must have it that way. For this purpose a day class might be the solution.
    public class TimeRegistration
    {
        public OrderNumber OrderNumber { get; }

        public TimeRange Range { get; }

        public TimeRegistration(OrderNumber orderNumber, TimeRange range)
        {
            OrderNumber = orderNumber;
            Range = range;
        }
    }
}
