namespace DammeTime.Core.TimeReporting.Domain
{
    // TODO: How to add Date?
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
