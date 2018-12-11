namespace DammeTime.Core.TimeReporting.Domain
{
    public class OrderNumber
    {
        public string Reference { get; }

        public OrderNumber(string reference)
        {
            Reference = reference;
        }
    }
}