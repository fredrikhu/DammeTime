using System;

namespace DammeTime.Core.TimeReporting.Domain
{
    public class OrderNumber : IEquatable<OrderNumber>
    {
        public string Reference { get; }

        public OrderNumber(string reference)
        {
            Reference = reference;
        }

        public bool Equals(OrderNumber other)
        {
            return Reference.Equals(other.Reference);
        }
    }
}
