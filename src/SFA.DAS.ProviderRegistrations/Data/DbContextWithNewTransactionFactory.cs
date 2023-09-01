using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRegistrations.Data;

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
            .UseSqlServer(_dbConnection);
                
        if (_environmentService.IsCurrent(DasEnv.LOCAL))
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        var dbContext = new ProviderRegistrationsDbContext(optionsBuilder.Options);

        return dbContext;
    }
}