using System;
using System.Linq;
using System.Threading;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration;
using DammeTime.Core.TimeReporting.Domain.Exceptions;
using DammeTime.Core.TimeReporting.Events;
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

        public AddTimeRegistrationHandlerTests()
        {
            Handler();
            Command();
        }

        public class Any_input_event : AddTimeRegistrationHandlerTests
        {
            [Fact]
            public async void is_saved_with_the_same_id_as_provided()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Id, ActualInputEvent.Id);
            }

            [Fact]
            public async void is_saved_with_the_same_start_time_as_provided()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Start, ActualInputEvent.Start);
            }

            [Fact]
            public async void is_saved_with_the_same_duration_as_provided()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Duration, ActualInputEvent.Duration);
            }
        }

        public class An_input_event_with_a_new_order_number : AddTimeRegistrationHandlerTests
        {
            [Fact]
            public async void adds_the_new_order_number()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.OrderNumber, ActualOrderNumberDomainEvent.OrderNumber);
            }

            [Fact]
            public async void saves_a_domain_event_with_same_order_number()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ActualOrderNumberDomainEvent.Id, ActualDomainEvent.OrderNumberId);
            }
        }

        public class An_input_event_with_a_pre_existing_order_number : AddTimeRegistrationHandlerTests
        {
            [Fact]
            public async void does_not_add_a_new_order_number()
            {
                AddOrderNumberOfCommand();

                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(1, _context.AddOrderNumberDomainEvents.Count(x => x.OrderNumber == _command.Event.OrderNumber));
            }

            [Fact]
            public async void saves_a_domain_event_with_same_order_number()
            {
                AddOrderNumberOfCommand();

                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ActualOrderNumberDomainEvent.Id, ActualDomainEvent.OrderNumberId);
            }

            private void AddOrderNumberOfCommand()
            {
                _context.AddOrderNumberDomainEvents.Add(new AddOrderNumberDomainEvent
                {
                    Id = Guid.NewGuid(),
                    OrderNumber = _command.Event.OrderNumber
                });

                _context.SaveChanges();
            }
        }

        public class A_valid_input_event : AddTimeRegistrationHandlerTests
        {
            [Fact]
            public async void saves_a_domain_event_with_same_id()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Id, ActualDomainEvent.Id);
            }

            [Fact]
            public async void saves_a_domain_event_with_same_start_time()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Start.TimeOfDay, ActualDomainEvent.Start);
            }

            [Fact]
            public async void saves_a_domain_event_with_stop_time_as_start_time_plus_duration()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Start.TimeOfDay.Add(ExpectedInputEvent.Duration), ActualDomainEvent.Stop);
            }
        }

        public class An_input_event_with_invalid_range : AddTimeRegistrationHandlerTests
        {
            [Fact]
            public async void throws_an_exception()
            {
                _command.Event.Duration = new TimeSpan(-5, 0, 0);

                var ex = await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.IsType<StopIsNotAfterStart>(ex);
            }

            [Fact]
            public async void does_not_save_domain_event()
            {
                _command.Event.Duration = new TimeSpan(-5, 0, 0);

                await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.Null(ActualDomainEvent);
            }

            [Fact]
            public async void does_not_add_order_number()
            {
                _command.Event.Duration = new TimeSpan(-5, 0, 0);

                await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.Null(ActualOrderNumberDomainEvent);
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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
                    Duration = new TimeSpan(3, 30, 0),
                    OrderNumber = "AB711 12345"
                }
            );
        }

        protected AddOrderNumberDomainEvent ActualOrderNumberDomainEvent => _context.AddOrderNumberDomainEvents.SingleOrDefault();
        protected AddTimeRegistrationDomainEvent ActualDomainEvent => _context.TimeRegistrationDomainEvents.SingleOrDefault();
        protected AddTimeRegistrationEvent ActualInputEvent => _context.TimeRegistrationEvents.Single();
        protected AddTimeRegistrationEvent ExpectedInputEvent => _command.Event;
    }
}