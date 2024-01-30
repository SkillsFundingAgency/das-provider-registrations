using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.SqlServer.Data;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.Data;

public interface IProviderRegistrationsDbContextFactory
{
    ProviderRegistrationsDbContext CreateDbContext(bool isDevOrLocal);
}

public class DbContextWithNServiceBusTransactionFactory : IProviderRegistrationsDbContextFactory
{
    private readonly IUnitOfWorkContext _unitOfWorkContext;
    private readonly ILoggerFactory _loggerFactory;

    public DbContextWithNServiceBusTransactionFactory(IUnitOfWorkContext unitOfWorkContext, ILoggerFactory loggerFactory)
    {
        _unitOfWorkContext = unitOfWorkContext;
        _loggerFactory = loggerFactory;
    }

    public ProviderRegistrationsDbContext CreateDbContext(bool isDevOrLocal)
    {
        var synchronizedStorageSession = _unitOfWorkContext.Find<SynchronizedStorageSession>();
        var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();

        var optionsBuilder = new DbContextOptionsBuilder<ProviderRegistrationsDbContext>()
            .UseSqlServer(sqlStorageSession.Connection);

        if (isDevOrLocal)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        var dbContext = new ProviderRegistrationsDbContext(optionsBuilder.Options);

        dbContext.Database.UseTransaction(sqlStorageSession.Transaction);

        return dbContext;
    }
}