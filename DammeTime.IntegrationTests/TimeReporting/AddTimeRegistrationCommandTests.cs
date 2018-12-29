// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DammeTime.Core.TimeReporting.Application.Commands.AddTimeRegistration;
// using MediatR;
// using Xunit;

// namespace DammeTime.IntegrationTests.TimeReporting
// {
//     public class Test
//     {
//         // TODO: Tests will probably be:
//         // TODO: 1. Use IoC-container config
//         // TODO: 2. Configure EF Core to use in memory database? Override config with mock?
//         // TODO: 3. Send command
//         // TODO: 4. Verify that command has performed action
//         public object SF(Type serviceType)
//         {
//             // var enumerableType = serviceType
//             //     .GetInterfaces()
//             //     .Concat(new [] {serviceType})
//             //     .FirstOrDefault(t =>  t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

//             // if (enumerableType != null)
//             //     return new List<IPipelineBehavior<AddTimeRegistrationCommand, Unit>>(); /* k.ResolveAll(enumerableType.GetGenericArguments()[0])*/
                
//             // return new AddTimeRegistrationHandler(null);
//                         //return new AddTimeRegistrationHandler(null);
//         }

//         [Fact]
//         public async void Testing()
//         {
            
//             //var m = new Mediator(SF);
//             //await m.Send(new AddTimeRegistrationCommand(null));
//         }
//     }
// }