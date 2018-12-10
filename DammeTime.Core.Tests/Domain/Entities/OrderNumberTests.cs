using System;
using DammeTime.Core.Domain.Entities;
using Xunit;

namespace DammeTime.Core.Tests.Domain.Entities
{
    public class OrderNumberTests
    {
        private OrderNumber _sut;
        private string _reference;

        [Fact]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Sets_Reference_To_Provided_Value()
        {
            var reference = "ref";
            OrderNumberWithReference(reference);

            var actual = _sut.Reference;

            Assert.Equal(reference, _reference);
        }

        private void OrderNumberWithReference(string reference)
        {
            _reference = reference;
            _sut = new OrderNumber(_reference);
        }

        [Theory]
        [InlineData("ref1", "ref1")]
        [InlineData("ref2", "ref2")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Equals_If_References_Are_Equal(string ref1, string ref2)
        {
            OrderNumberWithReference(ref1);
            var other = new OrderNumber(ref2);

            var actual = _sut.Equals(other);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("ref1", "ref2")]
        [InlineData("ref2", "ref3")]
        [Trait("Type", "UnitTest")]
        [Trait("Area", "Domain")]
        public void Does_Not_Equal_If_References_Are_Not_Equal(string ref1, string ref2)
        {
            OrderNumberWithReference(ref1);
            var other = new OrderNumber(ref2);

            var actual = _sut.Equals(other);

            Assert.False(actual);
        }
    }
}
