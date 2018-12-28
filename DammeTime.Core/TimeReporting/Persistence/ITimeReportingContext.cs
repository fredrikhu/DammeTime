using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration;
using Microsoft.EntityFrameworkCore;

namespace DammeTime.Core.TimeReporting.Persistence
{
    public interface ITimeReportingContext : IContext
    {
        DbSet<AddTimeRegistrationEvent> TimeRegistrationEvents { get; }
    }
}