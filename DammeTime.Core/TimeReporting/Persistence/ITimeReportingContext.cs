using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;
using Microsoft.EntityFrameworkCore;

namespace DammeTime.Core.TimeReporting.Persistence
{
    public interface ITimeReportingContext : IContext
    {
        DbSet<AddTimeRegistrationInputEvent> TimeRegistrationEvents { get; }
        DbSet<AddTimeRegistrationDomainEvent> TimeRegistrationDomainEvents { get; }
        DbSet<AddOrderNumberDomainEvent> AddOrderNumberDomainEvents { get; }
    }
}