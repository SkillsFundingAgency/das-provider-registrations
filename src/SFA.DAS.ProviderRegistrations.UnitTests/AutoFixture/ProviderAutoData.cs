using AutoFixture;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture
{
    public class ProviderAutoDataAttribute : AutoDataAttribute
    {
        public ProviderAutoDataAttribute() : base(() => new Fixture().Customize(new DomainCustomizations()))
        {
        }
    }
}
