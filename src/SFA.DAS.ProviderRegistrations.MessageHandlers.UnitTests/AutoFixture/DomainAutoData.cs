using AutoFixture;
using AutoFixture.NUnit3;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture;

public class DomainAutoDataAttribute : AutoDataAttribute
{
    public DomainAutoDataAttribute() : base(() => new Fixture().Customize(new DomainCustomizations()))
    {
    }
}