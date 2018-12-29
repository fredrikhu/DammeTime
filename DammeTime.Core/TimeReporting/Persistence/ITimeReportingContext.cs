using DammeTime.Core.TimeReporting.Events;
using Microsoft.EntityFrameworkCore;

namespace DammeTime.Core.TimeReporting.Persistence
{
    public interface ITimeReportingContext : IContext
    {
        DbSet<AddTimeRegistrationEvent> TimeRegistrationEvents { get; }
        DbSet<AddTimeRegistrationDomainEvent> TimeRegistrationDomainEvents { get; }
        DbSet<AddOrderNumberDomainEvent> AddOrderNumberDomainEvents { get; }
    }
}