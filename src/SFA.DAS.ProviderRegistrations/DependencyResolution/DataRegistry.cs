using System.Data.Common;
using System.Data.SqlClient;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRegistrationsSettings>().DatabaseConnectionString));
            For<ProviderRegistrationsDbContext>().Use(c => c.GetInstance<IProviderRegistrationsDbContextFactory>().CreateDbContext());
        }
    }
}