using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRegistrations.Data
{
    public class DbContextWithNewTransactionFactory : IProviderRegistrationsDbContextFactory
    {
        private readonly DbConnection _dbConnection;
        private readonly IEnvironmentService _environmentService;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextWithNewTransactionFactory(DbConnection dbConnection, IEnvironmentService environmentService, ILoggerFactory loggerFactory)
        {
            _dbConnection = dbConnection;
            _environmentService = environmentService;
            _loggerFactory = loggerFactory;
        }

        public ProviderRegistrationsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRegistrationsDbContext>()
                .UseSqlServer(_dbConnection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            if (_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }

            var dbContext = new ProviderRegistrationsDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}