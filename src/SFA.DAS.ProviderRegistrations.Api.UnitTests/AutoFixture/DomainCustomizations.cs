using AutoFixture;
using AutoFixture.AutoMoq;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests.AutoFixture;

public class DomainCustomizations : CompositeCustomization
{
    public DomainCustomizations() : base(new AutoMoqCustomization { ConfigureMembers = true })
    {
    }
}