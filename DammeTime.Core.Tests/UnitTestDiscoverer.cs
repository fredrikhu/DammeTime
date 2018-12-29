using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DammeTime.Core.Tests
{
    public class UnitTestDiscoverer : ITraitDiscoverer
    {
        internal const string AssemblyName = nameof(DammeTime) + "." + nameof(DammeTime.Core) + "." + nameof(DammeTime.Core.Tests);
        internal const string DiscovererTypeName = AssemblyName + "." + nameof(UnitTestDiscoverer);

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>("Category", "Unit Test");
        }
    }
}
