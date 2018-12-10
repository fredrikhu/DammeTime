using System;
using System.Globalization;
using Xunit;

namespace DammeTime.Core.Tests.Culture
{
    public class TemporaryCultureTests
    {
        private TemporaryCulture _sut;

        [Fact]
        public void Sets_Culture_Upon_Creation()
        {
            using (WithTemporaryCulture("en-US"))
            {
                Assert.Equal("en-US", CultureInfo.CurrentCulture.Name);
                Assert.Equal("en-US", CultureInfo.CurrentUICulture.Name);
            }
        }

        [Fact]
        public void Restores_Culture_When_Disposing()
        {
            using (WithTemporaryCulture("sv-SE"))
            {
                using (WithTemporaryCulture("en-US"))
                {

                }

                Assert.Equal("sv-SE", CultureInfo.CurrentCulture.Name);
                Assert.Equal("sv-SE", CultureInfo.CurrentUICulture.Name);
            }
        }

        [Fact]
        public void Restores_Current_Culture_And_UI_Culture_Individually()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("en");
                CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

                using (WithTemporaryCulture("sv-SE"))
                {

                }

                Assert.Equal("en", CultureInfo.CurrentCulture.Name);
                Assert.Equal("en-US", CultureInfo.CurrentUICulture.Name);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        private IDisposable WithTemporaryCulture(string culture)
        {
            _sut = new TemporaryCulture(culture);
            return _sut;
        }
    }
}