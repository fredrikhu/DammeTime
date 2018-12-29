using DammeTime.Core.TimeReporting.Events;
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
        public DbSet<AddTimeRegistrationDomainEvent> TimeRegistrationDomainEvents { get; set; }
        public DbSet<AddOrderNumberDomainEvent> AddOrderNumberDomainEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DammeTimeContext).Assembly);
        }
    }
}