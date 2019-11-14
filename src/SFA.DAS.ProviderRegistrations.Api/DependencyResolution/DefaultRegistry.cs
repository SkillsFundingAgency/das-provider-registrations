using SFA.DAS.ProviderRegistrations.Data;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Api.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.ProviderRegistrations";

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<IProviderRegistrationsDbContextFactory>().Use<DbContextWithNewTransactionFactory>();
        }
    }
}
