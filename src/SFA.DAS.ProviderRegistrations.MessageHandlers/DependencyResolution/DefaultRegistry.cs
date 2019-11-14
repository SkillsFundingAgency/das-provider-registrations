using System;
using SFA.DAS.ProviderRegistrations.Data;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IProviderRegistrationsDbContextFactory>().Use<DbContextWithNServiceBusTransactionFactory>();
        }
    }
}
