using System.Linq;
using DammeTime.Core.Validation;
using Xunit;

namespace DammeTime.Core.Tests.Validation
{
    public class ValidationBaseTests
    {
        private ValidationMock _validation;

        public ValidationBaseTests()
        {
            _validation = new ValidationMock();
        }

        public class A_subject_with_no_errors : ValidationBaseTests
        {
            [Fact, UnitTest]
            public void throws_no_exception()
            {
                var subject = new DomainObject();

                var ex = Record.Exception(() => _validation.Validate(subject));

                Assert.Null(ex);
            }
        }

        public class A_subject_with_property_errors : ValidationBaseTests
        {
            [Fact, UnitTest]
            public void throws_an_exception_with_property_name()
            {
                var subject = new DomainObject { Property = -5 };

                var ex = Record.Exception(() => _validation.Validate(subject)) as ValidationError;
            
                Assert.Equal("Property", ex.Errors.Single().Property);
            }

            [Fact, UnitTest]
            public void throws_an_exception_with_error_message()
            {
                var subject = new DomainObject { Property = -5 };

                var ex = Record.Exception(() => _validation.Validate(subject)) as ValidationError;
            
                Assert.Equal("Property cannot be -5", ex.Errors.Single().Message);
            }
        }

        public class A_subject_with_entity_errors : ValidationBaseTests
        {
            [Fact, UnitTest]
            public void throws_an_exception_without_property_name()
            {
                var subject = new DomainObject { Error = true };

                var ex = Record.Exception(() => _validation.Validate(subject)) as ValidationError;
            
                Assert.Equal("", ex.Errors.Single().Property);
            }

            [Fact, UnitTest]
            public void throws_an_exception_with_error_message()
            {
                var subject = new DomainObject { Error = true };

                var ex = Record.Exception(() => _validation.Validate(subject)) as ValidationError;
            
                Assert.Equal("Error", ex.Errors.Single().Message);
            }
        }

        public class A_subject_with_entity_and_property_errors : ValidationBaseTests
        {
            [Fact, UnitTest]
            public void throws_an_exception_with_both_kinds_of_errors()
            {
                var subject = new DomainObject { Error = true, Property = -5 };

                var ex = Record.Exception(() => _validation.Validate(subject)) as ValidationError;
            
                Assert.Contains(ex.Errors, x => x.Property == "");
                Assert.Contains(ex.Errors, x => x.Property == "Property");
            }
        }

        private class ValidationMock : ValidationBase<DomainObject>
        {
            public ValidationMock()
            {
                Property(x => x.Property)
                    .ShallSatisfy(x => x >= 0)
                    .ElseErrorMessage((n, v) => $"{n} cannot be {v}");

                Entity()
                    .ShallSatisfy(x => !x.Error)
                    .ElseErrorMessage(x => "Error");
            }
        }

        private class DomainObject
        {
            public int Property { get; set; }
            public bool Error { get; set; }
        }
    }
}