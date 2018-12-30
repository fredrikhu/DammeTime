using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DammeTime.Core.Validation
{
    public abstract partial class ValidationBase<T>
    {
        private class PropertyVerification<TProperty> : IPropertyVerification<T, TProperty>
        {
            private readonly Expression<Func<T, TProperty>> _property;
            private Func<TProperty, bool> _predicate;
            private Func<string, TProperty, string> _message;

            public PropertyVerification(Expression<Func<T, TProperty>> property)
            {
                if (property == null) throw new ArgumentNullException(nameof(property));
                if (property.Body as MemberExpression == null) throw new ArgumentException($"{nameof(property)} is not a property expression.");

                _property = property;
            }

            public IPropertyVerification<T, TProperty> ShallSatisfy(Func<TProperty, bool> predicate)
            {
                _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
                return this;
            }

            public IPropertyVerification<T, TProperty> ElseErrorMessage(Func<string, TProperty, string> message)
            {
                _message = message ?? throw new ArgumentNullException(nameof(message));
                return this;
            }

            public IEnumerable<ValidationError.Error> Validate(T subject)
            {
                var value = _property.Compile().Invoke(subject);
                if (!_predicate(value))
                {
                    var name = ((MemberExpression)_property.Body)?.Member.Name ?? string.Empty;                    
                    yield return new ValidationError.Error(name, _message(name, value));
                }
            }
        }
    }
}