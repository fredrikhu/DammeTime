using System.Reflection;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration;
using DammeTime.Core.TimeReporting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DammeTime.Persistence
{
    public class DammeTimeContext : DbContext, ITimeReportingContext
    {
        public DammeTimeContext()
        { }

        public DammeTimeContext(DbContextOptions<DammeTimeContext> options)
            : base(options)
        { }

        public DbSet<AddTimeRegistrationEvent> TimeRegistrationEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DammeTimeContext).Assembly);
        }
    }
}