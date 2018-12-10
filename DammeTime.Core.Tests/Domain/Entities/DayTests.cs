using System;
using System.Collections.Generic;
using System.Linq;
using DammeTime.Core.Domain.Entities;
using DammeTime.Core.Domain.Exceptions;
using Xunit;

namespace DammeTime.Core.Tests.Domain.Entities
{
    public class DayTests
    {
        private Day _sut;
        private Date _date;

        public class A_new_day : DayTests
        {
            [Fact]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void has_the_date_provided()
            {
                DayWithDefaultDate();

                var actual = _sut.Date;

                Assert.Equal(_date, actual);
            }

            [Theory]
            [MemberData(nameof(contains_the_added_registration_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void contains_the_added_registration(Guid guid, string reference, string start, string stop)
            {
                DayWithDefaultDate();

                AddRegistration(guid, reference, start, stop);

                ContainsRegistration(guid, reference, start, stop);
            }

            public static IEnumerable<object[]> contains_the_added_registration_Data()
            {
                return new List<object[]>
                {
                    new object[] { Guid.NewGuid(), "ref", "08:00", "10:00" },
                    new object[] { Guid.NewGuid(), "ref", "10:00", "12:00" }
                };
            }

            [Fact]
            public void has_start_and_end_time_at_midnight()
            {
                var start = "00:00";
                var stop = "00:00";

                DayWithDefaultDate();

                StartOfDayEquals(start);
                EndOfDayEquals(stop);
            }

            [Fact]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void updates_start_and_end_time_when_adding_first_registration()
            {
                DayWithDefaultDate();

                AddRegistration(Guid.NewGuid(), "ref", "08:00", "10:00");

                StartOfDayEquals("08:00");
                EndOfDayEquals("10:00");
            }
        }

        public class A_day_with_registrations : DayTests
        {
            [Fact]
            public void discards_removed_registration()
            {
                // Arrange
                string reference = "ref";
                var start = "10:00";
                var stop = "12:00";
                var guid = Guid.NewGuid();

                DayWithDefaultDate();
                AddRegistration(guid, reference, start, stop);

                // Act
                RemoveRegistration(guid);

                // Assert
                DoesNotContainRegistration(guid);
            }

            [Theory]
            [MemberData(nameof(only_updates_end_time_when_adding_a_registration_after_all_previous_registrations_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void only_updates_end_time_when_adding_a_registration_after_all_previous_registrations(string start, string end)
            {
                DayWithDefaultDate();
                AddRegistration(Guid.NewGuid(), "ref", "08:00", "10:00");

                AddRegistration(Guid.NewGuid(), "ref", start, end);

                StartOfDayEquals("08:00");
                EndOfDayEquals(end);
            }

            public static IEnumerable<object[]> only_updates_end_time_when_adding_a_registration_after_all_previous_registrations_Data()
            {
                return new List<object[]>
                {
                    new object[] { "10:00", "12:00" },
                    new object[] { "11:00", "12:00" },
                };
            }

            [Theory]
            [MemberData(nameof(only_updates_start_time_when_adding_a_registration_before_all_previous_registrations_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void only_updates_start_time_when_adding_a_registration_before_all_previous_registrations(string start, string end)
            {
                DayWithDefaultDate();
                AddRegistration(Guid.NewGuid(), "ref", "12:00", "14:00");

                AddRegistration(Guid.NewGuid(), "ref", start, end);

                StartOfDayEquals(start);
                EndOfDayEquals("14:00");
            }

            public static IEnumerable<object[]> only_updates_start_time_when_adding_a_registration_before_all_previous_registrations_Data()
            {
                return new List<object[]>
                {
                    new object[] { "10:00", "11:00" },
                    new object[] { "10:00", "12:00" },
                };
            }

            [Theory]
            [MemberData(nameof(throws_an_excpetion_when_adding_a_registration_that_encloses_or_is_enclosed_by_previous_registration_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void throws_an_excpetion_when_adding_a_registration_that_encloses_or_is_enclosed_by_previous_registration(string reference, Guid guid1, string start1, string stop1, Guid guid2, string start2, string stop2)
            {
                // Arrange
                DayWithDefaultDate();
                AddRegistration(guid1, reference, start1, stop1);

                // Act
                var ex = Record.Exception(() => AddRegistration(guid2, reference, start2, stop2));

                // Assert
                Assert.NotNull(ex);
                Assert.IsType<InvalidOrderRegistration>(ex);
            }

            public static IEnumerable<object[]> throws_an_excpetion_when_adding_a_registration_that_encloses_or_is_enclosed_by_previous_registration_Data()
            {
                return new List<object[]>
                {
                    new object[] { "ref", Guid.NewGuid(), "08:00", "10:00", Guid.NewGuid(), "09:00", "09:30" },
                    new object[] { "ref", Guid.NewGuid(), "09:00", "09:30", Guid.NewGuid(), "08:00", "10:00" }
                };
            }

            [Theory]
            [MemberData(nameof(throws_an_exception_when_adding_a_registration_that_overlaps_previous_registration_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void throws_an_exception_when_adding_a_registration_that_overlaps_previous_registration(string reference, Guid guid1, string start1, string stop1, Guid guid2, string start2, string stop2)
            {
                // Arrange
                DayWithDefaultDate();
                AddRegistration(guid1, reference, start1, stop1);

                // Act
                var ex = Record.Exception(() => AddRegistration(guid2, reference, start2, stop2));

                // Assert
                Assert.NotNull(ex);
                Assert.IsType<InvalidOrderRegistration>(ex);
            }

            public static IEnumerable<object[]> throws_an_exception_when_adding_a_registration_that_overlaps_previous_registration_Data()
            {
                return new List<object[]>
                {
                    new object[] { "ref", Guid.NewGuid(), "08:00", "10:00", Guid.NewGuid(), "09:00", "11:00" },
                    new object[] { "ref", Guid.NewGuid(), "09:00", "11:00", Guid.NewGuid(), "08:00", "10:00" }
                };
            }
            
            [Theory]
            [MemberData(nameof(does_not_throw_exception_when_adding_a_registration_that_is_adjacent_to_a_previous_registration_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void does_not_throw_exception_when_adding_a_registration_that_is_adjacent_to_a_previous_registration(string start, string stop)
            {
                // Arrange
                DayWithDefaultDate();
                AddRegistration(Guid.NewGuid(), "ref", "08:00", "10:00");

                // Act
                var ex = Record.Exception(() => AddRegistration(Guid.NewGuid(), "ref", start, stop));

                // Assert
                Assert.Null(ex);
            }

            public static IEnumerable<object[]> does_not_throw_exception_when_adding_a_registration_that_is_adjacent_to_a_previous_registration_Data()
            {
                return new List<object[]>
                {
                    new object[] { "10:00", "11:00" },
                    new object[] { "07:00", "08:00" }
                };
            }

            [Theory]
            [MemberData(nameof(Does_Not_Throw_Exception_If_Registrations_Are_Not_Adjacent_To_One_Another_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void does_not_throw_exception_when_adding_a_registration_that_is_not_adjacent_to_a_previous_registration(string start, string stop)
            {
                // Arrange
                DayWithDefaultDate();
                AddRegistration(Guid.NewGuid(), "ref", "08:00", "10:00");

                // Act
                var ex = Record.Exception(() => AddRegistration(Guid.NewGuid(), "ref", start, stop));

                // Assert
                Assert.Null(ex);
            }

            public static IEnumerable<object[]> Does_Not_Throw_Exception_If_Registrations_Are_Not_Adjacent_To_One_Another_Data()
            {
                return new List<object[]>
                {
                    new object[] { "11:00", "12:00" },
                    new object[] { "06:00", "07:00" }
                };
            }
        }

        public class Any_day : DayTests
        {
            [Theory]
            [MemberData(nameof(orders_registration_by_start_time_Data))]
            [Trait("Type", "UnitTest")]
            [Trait("Area", "Domain")]
            public void orders_registration_by_start_time(string reference, Guid guid1, string start1, string stop1, Guid guid2, string start2, string stop2)
            {
                DayWithDefaultDate();

                AddRegistration(guid1, reference, start1, stop1);
                AddRegistration(guid2, reference, start2, stop2);

                FirstOrderRegistrationStartsAt(MinimumTime(start1, start2));
            }

            public static IEnumerable<object[]> orders_registration_by_start_time_Data()
            {
                return new List<object[]>
                {
                    new object[] { "ref", Guid.NewGuid(), "08:00", "10:00", Guid.NewGuid(), "11:00", "12:00" },
                    new object[] { "ref", Guid.NewGuid(), "11:00", "12:00", Guid.NewGuid(), "08:00", "10:00" }
                };
            }

            private TimeSpan MinimumTime(string time1, string time2)
            {
                var ts1 = TimeSpan.Parse(time1);
                var ts2 = TimeSpan.Parse(time2);

                return ts1 < ts2 ? ts1 : ts2;
            }

            private void FirstOrderRegistrationStartsAt(TimeSpan time)
            {
                Assert.Equal(time, _sut.OrderRegistrations.First().Start);
            }
        }

        private void DayWithDefaultDate()
        {
            _date = new Date(2000, 10, 1);
            _sut = new Day(_date);
        }

        private void AddRegistration(Guid guid, string reference, string start, string stop)
        {
            var orderNumber = new OrderNumber(reference);
            var range = new TimeRange(start, stop);
            var registration = new OrderRegistration(guid, orderNumber, range);

            _sut.AddOrderRegistration(registration);
        }

        private void RemoveRegistration(Guid guid)
        {
            _sut.RemoveOrderRegistration(guid);
        }

        private void DoesNotContainRegistration(Guid guid)
        {
            Assert.DoesNotContain(_sut.OrderRegistrations, x => x.Guid == guid);
        }

        private void ContainsRegistration(Guid guid, string reference, string start, string stop)
        {
            Assert.Collection(_sut.OrderRegistrations,
                x => {
                    Assert.Equal(guid, x.Guid);
                    Assert.Equal(new OrderNumber(reference), x.OrderNumber);
                    Assert.Equal(TimeSpan.Parse(start), x.Start);
                    Assert.Equal(TimeSpan.Parse(stop), x.Stop);
                });
        }

        private void StartOfDayEquals(string start)
        {
            Assert.Equal(_sut.StartOfDay, TimeSpan.Parse(start));
        }

        private void EndOfDayEquals(string end)
        {
            Assert.Equal(_sut.EndOfDay, TimeSpan.Parse(end));
        }
    }
}
