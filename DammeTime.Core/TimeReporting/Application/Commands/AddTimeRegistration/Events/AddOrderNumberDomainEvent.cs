using System;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events
{
    public class AddOrderNumberDomainEvent
    {
        public Guid Id { get; set; }
        public int EventOrder { get; set; }
        public string OrderNumber { get; set; }
    }
}