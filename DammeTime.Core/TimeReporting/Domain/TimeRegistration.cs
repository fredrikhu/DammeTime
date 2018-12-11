namespace DammeTime.Core.TimeReporting.Domain
{
    public class TimeRegistration
    {
        public OrderNumber OrderNumber { get; }

        public TimeRegistration(OrderNumber orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}