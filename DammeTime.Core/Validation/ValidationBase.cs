using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;

namespace DammeTime.Core.Validation
{
    public abstract partial class ValidationBase<T>
    {
        private List<IVerification<T>> _verifications = new List<IVerification<T>>();

        public void Validate(T @event)
        {
            var validationErrors = _verifications.SelectMany(x => x.Validate(@event));

            if (validationErrors.Any())
                throw new ValidationError(validationErrors.ToArray());
        }

        protected IPropertyVerification<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> x)
        {
            var v = new PropertyVerification<TProperty>(x);
            _verifications.Add(v);
            return v;
        }

        protected IEntityVerification<T> Entity()
        {
            var v = new EntityVerification();
            _verifications.Add(v);
            return v;
        }
    }
}