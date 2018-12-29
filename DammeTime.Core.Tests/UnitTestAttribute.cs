using System;
using Xunit.Sdk;

namespace DammeTime.Core.Tests
{
    [TraitDiscoverer(UnitTestDiscoverer.DiscovererTypeName, UnitTestDiscoverer.AssemblyName)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class UnitTestAttribute : Attribute, ITraitAttribute
    {
        public UnitTestAttribute()
            : base()
        {

        }
    }
}
