using System;

namespace DammeTime.Core.TimeReporting.Events
{
    public class AddOrderNumberDomainEvent
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
    }
}