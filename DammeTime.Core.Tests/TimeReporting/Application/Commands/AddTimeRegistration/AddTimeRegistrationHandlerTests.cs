using System;
using System.Linq;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration;
using DammeTime.Core.TimeReporting.Persistence;
using DammeTime.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DammeTime.Core.Tests.TimeReporting.Application.Commands.AddTimeRegistration
{
    public class AddTimeRegistrationHandlerTests
    {
        protected AddTimeRegistrationHandler _handler;
        protected AddTimeRegistrationCommand _command;
        protected DammeTimeContext _context;

        public class A_command_with_an_event_is_saved : AddTimeRegistrationHandlerTests
        {
            public A_command_with_an_event_is_saved()
            {
                Handler();
                Command();
            }

            [Fact]
            public void with_the_same_id_as_provided()
            {
                _handler.Handle(_command);

                Assert.Equal(Expected.Id, Actual.Id);
            }

            [Fact]
            public void with_the_same_start_time_as_provided()
            {
                _handler.Handle(_command);

                Assert.Equal(Expected.Start, Actual.Start);
            }

            [Fact]
            public void with_the_same_duration_as_provided()
            {
                _handler.Handle(_command);

                Assert.Equal(Expected.Duration, Actual.Duration);
            }
        }

        protected void Handler()
        {
            Context();
            _handler = new AddTimeRegistrationHandler(_context);
        }

        protected void Context()
        {
            var options = new DbContextOptionsBuilder<DammeTimeContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;

            _context = new DammeTimeContext(options);
            _context.Database.EnsureDeleted();
        }

        protected void Command()
        {
            _command = new AddTimeRegistrationCommand(new AddTimeRegistrationEvent
                {
                    Id = Guid.NewGuid(),
                    Start = new DateTimeOffset(2010, 01, 02, 03, 04, 05, new TimeSpan(6, 0, 0)),
                    Duration = new TimeSpan(3, 30, 0)
                }
            );
        }

        protected AddTimeRegistrationEvent Actual => _context.TimeRegistrationEvents.Single();
        protected AddTimeRegistrationEvent Expected => _command.Event;
    }
}