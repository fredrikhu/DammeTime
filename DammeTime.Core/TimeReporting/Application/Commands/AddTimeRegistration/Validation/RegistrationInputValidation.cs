using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Events;
using DammeTime.Core.Validation;

namespace DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration.Validation
{
    public class RegistrationInputValidation : ValidationBase<AddTimeRegistrationInputEvent>
    {
        public RegistrationInputValidation()
        {
            Property(x => x.OrderNumber)
                .ShallSatisfy(x => !string.IsNullOrWhiteSpace(x))
                .ElseErrorMessage((n, v) => $"{n} cannot be empty.");
            Entity()
                .ShallSatisfy(x => x.Start.TimeOfDay < x.Stop)
                .ElseErrorMessage(x => "Stop time must be after start time.");
        }
    }
}
