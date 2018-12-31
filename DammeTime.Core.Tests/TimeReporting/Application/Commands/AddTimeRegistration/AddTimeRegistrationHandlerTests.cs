using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Validation;
using DammeTime.Core.TimeReporting.Domain.Exceptions;
using DammeTime.Core.Validation;
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

        protected AddOrderNumberDomainEvent ActualOrderNumberDomainEvent => _context.AddOrderNumberDomainEvents.SingleOrDefault();
        protected AddTimeRegistrationDomainEvent ActualDomainEvent => _context.TimeRegistrationDomainEvents.SingleOrDefault();
        protected AddTimeRegistrationInputEvent ActualInputEvent => _context.TimeRegistrationEvents.Single();
        protected AddTimeRegistrationInputEvent ExpectedInputEvent => _command.Event;

        public AddTimeRegistrationHandlerTests()
        {
            Handler();
            Command();
        }

        public class Any_input_event : AddTimeRegistrationHandlerTests
        {
            [Fact, UnitTest]
            public async void is_saved_with_the_same_id_as_provided()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Id, ActualInputEvent.Id);
            }

            [Fact, UnitTest]
            public async void is_saved_with_the_same_start_time_as_provided()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Start, ActualInputEvent.Start);
            }

            [Fact, UnitTest]
            public async void is_saved_with_the_same_stop_time_as_provided()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Stop, ActualInputEvent.Stop);
            }
        }

        public class An_input_event_with_a_new_order_number : AddTimeRegistrationHandlerTests
        {
            [Fact, UnitTest]
            public async void adds_the_new_order_number()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.OrderNumber, ActualOrderNumberDomainEvent.OrderNumber);
            }

            [Fact, UnitTest]
            public async void saves_a_domain_event_with_same_order_number()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ActualOrderNumberDomainEvent.Id, ActualDomainEvent.OrderNumberId);
            }
        }

        public class An_input_event_with_a_pre_existing_order_number : AddTimeRegistrationHandlerTests
        {
            [Fact, UnitTest]
            public async void does_not_add_a_new_order_number()
            {
                AddOrderNumberOfCommand();

                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(1, _context.AddOrderNumberDomainEvents.Count(x => x.OrderNumber == _command.Event.OrderNumber));
            }

            [Fact, UnitTest]
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
            [Fact, UnitTest]
            public async void saves_a_domain_event_with_same_id()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Id, ActualDomainEvent.Id);
            }

            [Fact, UnitTest]
            public async void saves_a_domain_event_with_same_start_time()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Start.TimeOfDay, ActualDomainEvent.Start);
            }

            [Fact, UnitTest]
            public async void saves_a_domain_event_with_same_stop_time()
            {
                await _handler.Handle(_command, CancellationToken.None);

                Assert.Equal(ExpectedInputEvent.Stop, ActualDomainEvent.Stop);
            }
        }

        public class An_input_event_with_invalid_range : AddTimeRegistrationHandlerTests
        {
            [Theory, UnitTest]
            [MemberData(nameof(HourOffsetData))]
            public async void throws_a_validation_exception(int hourOffset)
            {
                OffsetEndFromStartWith(hourOffset);

                var ex = await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.IsType<ValidationError>(ex);
                Assert.Equal("", ((ValidationError)ex).Errors.Single().Property);
                Assert.Equal("Stop time must be after start time.", ((ValidationError)ex).Errors.Single().Message);
            }

            [Theory, UnitTest]
            [MemberData(nameof(HourOffsetData))]
            public async void does_not_save_domain_event(int hourOffset)
            {
                OffsetEndFromStartWith(hourOffset);

                await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.Null(ActualDomainEvent);
            }

            [Theory, UnitTest]
            [MemberData(nameof(HourOffsetData))]
            public async void does_not_add_order_number(int hourOffset)
            {
                OffsetEndFromStartWith(hourOffset);

                await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.Null(ActualOrderNumberDomainEvent);
            }

            public static IEnumerable<object[]> HourOffsetData()
            {
                return new List<object[]>
                {
                    new object[] { 0 },
                    new object[] { -5 }
                };
            }

            private void OffsetEndFromStartWith(int hours)
            {
                _command.Event.Stop = _command.Event.Start.TimeOfDay.Add(new TimeSpan(hours, 0, 0));
            }
        }

        public class An_input_event_with_an_empty_order_number : AddTimeRegistrationHandlerTests
        {
            // TODO: Tests for saving input events
            [Theory, UnitTest]
            [MemberData(nameof(HourOffsetData))]
            public async void throws_a_validation_exception(string orderNumber)
            {
                OrderNumberIs(orderNumber);

                var ex = await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.IsType<ValidationError>(ex);
                Assert.Equal(nameof(_command.Event.OrderNumber), ((ValidationError)ex).Errors.Single().Property);
                Assert.Equal($"{nameof(_command.Event.OrderNumber)} cannot be empty.", ((ValidationError)ex).Errors.Single().Message);
            }

            [Theory, UnitTest]
            [MemberData(nameof(HourOffsetData))]
            public async void does_not_save_domain_event(string orderNumber)
            {
                OrderNumberIs(orderNumber);

                await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.Null(ActualDomainEvent);
            }

            [Theory, UnitTest]
            [MemberData(nameof(HourOffsetData))]
            public async void does_not_add_order_number(string orderNumber)
            {
                OrderNumberIs(orderNumber);

                await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.Null(ActualOrderNumberDomainEvent);
            }

            public static IEnumerable<object[]> HourOffsetData()
            {
                return new List<object[]>
                {
                    new object[] { "" },
                    new object[] { "   "},
                    new object[] { "\t"},
                };
            }

            private void OrderNumberIs(string orderNumber)
            {
                _command.Event.OrderNumber = orderNumber;
            }
        }

        public class An_input_with_invalid_range_and_order_number : AddTimeRegistrationHandlerTests
        {
            public An_input_with_invalid_range_and_order_number()
                : base()
            {
                _command.Event.OrderNumber = "";
                _command.Event.Stop = _command.Event.Start.TimeOfDay.Add(new TimeSpan(-5, 0, 0));
            }

            [Fact, UnitTest]
            public async void throws_a_validation_exception()
            {
                var ex = await Record.ExceptionAsync(async () => await _handler.Handle(_command, CancellationToken.None));

                Assert.IsType<ValidationError>(ex);
                Assert.Contains(((ValidationError)ex).Errors, x => x.Property == "");
                Assert.Contains(((ValidationError)ex).Errors, x => x.Property == nameof(_command.Event.OrderNumber));
            }
        }

        protected void Handler()
        {
            Context();
            _handler = new AddTimeRegistrationHandler(
                _context,
                new RegistrationInputValidation(),
                new OrderNumberDomainEventPersister(_context));
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
            _command = new AddTimeRegistrationCommand(new AddTimeRegistrationInputEvent
                {
                    Id = Guid.NewGuid(),
                    Start = new DateTimeOffset(2010, 01, 02, 03, 04, 05, new TimeSpan(6, 0, 0)),
                    Stop = new TimeSpan(06, 34, 05),
                    OrderNumber = "AB711 12345"
                }
            );
        }
    }
}