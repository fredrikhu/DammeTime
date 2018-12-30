using System;
using System.Collections.Generic;

namespace DammeTime.Core.Validation
{
    public abstract partial class ValidationBase<T>
    {
        private class EntityVerification : IEntityVerification<T>
        {
            private Func<T, bool> _predicate;
            private Func<T, string> _message;

            public IEntityVerification<T> ElseErrorMessage(Func<T, string> message)
            {
                _message = message ?? throw new ArgumentNullException(nameof(message));
                return this;
            }

            public IEntityVerification<T> ShallSatisfy(Func<T, bool> predicate)
            {
                _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
                return this;
            }

            public IEnumerable<ValidationError.Error> Validate(T subject)
            {
                if (!_predicate(subject))
                {
                    yield return new ValidationError.Error("", _message(subject));
                }
            }
        }
    }
}