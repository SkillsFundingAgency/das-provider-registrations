using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.StructureMap;
using SFA.DAS.UnitOfWork.NServiceBus.DependencyResolution.StructureMap;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DataRegistry>();
            registry.IncludeRegistry<EntityFrameworkCoreUnitOfWorkRegistry<ProviderRegistrationsDbContext>>();
            registry.IncludeRegistry<MediatorRegistry>();
            registry.IncludeRegistry<NServiceBusUnitOfWorkRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}
