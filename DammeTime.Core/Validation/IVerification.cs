using System.Collections.Generic;

namespace DammeTime.Core.Validation
{
    public interface IVerification<T>
    {
        IEnumerable<ValidationError.Error> Validate(T subject);
    }
}