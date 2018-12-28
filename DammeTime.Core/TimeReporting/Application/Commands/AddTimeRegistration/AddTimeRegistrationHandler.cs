using DammeTime.Core.TimeReporting.Persistence;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class AddTimeRegistrationHandler
    {
        private readonly ITimeReportingContext _context;

        public AddTimeRegistrationHandler(ITimeReportingContext context)
        {
            _context = context;
        }

        public void Handle(AddTimeRegistrationCommand command)
        {
            _context.TimeRegistrationEvents.Add(command.Event);
            _context.SaveChanges();
        }
    }
}