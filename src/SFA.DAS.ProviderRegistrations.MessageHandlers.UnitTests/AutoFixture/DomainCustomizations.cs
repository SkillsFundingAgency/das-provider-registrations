using AutoFixture;
using AutoFixture.AutoMoq;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture
{
    public class DomainCustomizations : CompositeCustomization
    {
        public DomainCustomizations() : base(
            new AutoMoqCustomization { ConfigureMembers = true },
            new MessageHandlerCustomizations())
        {
        }
    }
}
