using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.ProviderRegistrations.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<AuthorizationRegistry>();
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<MediatorRegistry>(); 
            registry.IncludeRegistry<DataRegistry>();
            registry.IncludeRegistry<MapperRegistry>();
            registry.IncludeRegistry<ServiceRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}