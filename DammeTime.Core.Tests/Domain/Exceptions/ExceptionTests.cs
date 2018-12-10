using System;
using System.Collections.Generic;
using System.Linq;
using DammeTime.Core.Domain.Exceptions;
using Xunit;

namespace DammeTime.Core.Tests.Domain.Exceptions
{
    public class ExceptionTests
    {
        private Exception _sut;

        [Theory]
        [MemberData(nameof(AllExceptions))]
        public void Default_Constructor_Has_Default_Message(Type type)
        {
            CreateSut(type);

            Assert.Equal($"Exception of type '{_sut.GetType().FullName}' was thrown.", _sut.Message);
        }

        [Theory]
        [MemberData(nameof(AllExceptions))]
        public void Default_Constructor_Has_No_Inner_Exception(Type type)
        {
            CreateSut(type);

            Assert.Null(_sut.InnerException);
        }

        private void CreateSut(Type type)
        {
            var ctor = type.GetConstructor(new Type[0]);
            _sut = (Exception)ctor.Invoke(new object[0]);
        }

        [Theory]
        [MemberData(nameof(AllExceptions))]
        public void Constructor_With_Message_Has_Provided_Message(Type type)
        {
            var message = "Message";
            CreateSut(type, message);

            Assert.Equal(message, _sut.Message);
        }

        [Theory]
        [MemberData(nameof(AllExceptions))]
        public void Constructor_With_Message_Has_No_Inner_Exception(Type type)
        {
            var message = "Message";
            CreateSut(type, message);

            Assert.Null(_sut.InnerException);
        }

        private void CreateSut(Type type, string message)
        {
            var ctor = type.GetConstructor(new Type[] { typeof(string) });
            _sut = (Exception)ctor.Invoke(new object[] { message });
        }

        [Theory]
        [MemberData(nameof(AllExceptions))]
        public void Constructor_With_Message_And_Exception_Has_Provided_Message(Type type)
        {
            var message = "Message";
            var exception = new Exception();
            CreateSut(type, message, exception);

            Assert.Equal(message, _sut.Message);
        }

        [Theory]
        [MemberData(nameof(AllExceptions))]
        public void Constructor_With_Message_And_Exception_Has_Provided_Inner_Exception(Type type)
        {
            var message = "Message";
            var exception = new Exception();
            CreateSut(type, message, exception);

            Assert.Equal(exception, _sut.InnerException);
        }

        private void CreateSut(Type type, string message, Exception exception)
        {
            var ctor = type.GetConstructor(new Type[] { typeof(string), typeof(Exception) });
            _sut = (Exception)ctor.Invoke(new object[] { message, exception });
        }

        public static IEnumerable<object[]> AllExceptions()
        {
            var type = typeof(DaysInWeekAreNotInTheSameWeek);

            return type.Assembly.GetTypes()
                    .Where(x => typeof(Exception).IsAssignableFrom(x))
                    .Select(x => new object[] { x });
        }
    }
}