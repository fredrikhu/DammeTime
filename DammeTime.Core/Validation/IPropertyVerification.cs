using System;

namespace DammeTime.Core.Validation
{
    public interface IPropertyVerification<T, TProperty> : IVerification<T>
    {
        IPropertyVerification<T, TProperty> ShallSatisfy(Func<TProperty, bool> predicate);
        IPropertyVerification<T, TProperty> ElseErrorMessage(Func<string, TProperty, string> message);
    }
}