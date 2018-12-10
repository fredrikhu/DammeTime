using System;
using System.Globalization;

namespace DammeTime.Core.Tests.Culture
{
    public class TemporaryCulture : IDisposable
    {
        private readonly CultureInfo _previousCulture;
        private readonly CultureInfo _previousUICulture;

        public TemporaryCulture(string culture)
        {
            _previousCulture = CultureInfo.CurrentCulture;
            _previousUICulture = CultureInfo.CurrentUICulture;

            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _previousCulture;
            CultureInfo.CurrentUICulture = _previousUICulture;
        }
    }
}