using SFA.DAS.ProviderRegistrations.Services;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution;

public class ServiceRegistry : Registry
{
    public ServiceRegistry()
    {
        For<IProviderService>().Use<ProviderService>();
        For<IEmployerUsersApiHttpClientFactory>().Use<EmployerApprenticeshipApiHttpClientFactory>();
        For<IRoatpApiHttpClientFactory>().Use<RoatpApiHttpClientFactory>();
        For<IEmployerApprenticeshipService>().Use<EmployerApprenticeshipService>();
    }
}