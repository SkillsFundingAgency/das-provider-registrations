using AutoFixture;
using NServiceBus.Testing;
using System;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture;

public class MessageHandlerCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new TestableMessageHandlerContext());
    }
}