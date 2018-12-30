using System;

namespace DammeTime.Core.Validation
{
    public interface IEntityVerification<T> : IVerification<T>
    {
        IEntityVerification<T> ShallSatisfy(Func<T, bool> predicate);
        IEntityVerification<T> ElseErrorMessage(Func<T, string> message);
    }
}