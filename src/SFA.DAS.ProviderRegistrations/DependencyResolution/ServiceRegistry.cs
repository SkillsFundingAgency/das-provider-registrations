using SFA.DAS.ProviderRegistrations.Services;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<IApprenticeshipInfoService>().Use<ApprenticeshipInfoService>();
        }
    }
}
