using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data;

public class ProviderRegistrationsDbContext : DbContext
{
    private readonly ProviderRegistrationsSettings _configuration;
    private readonly AzureServiceTokenProvider _azureServiceTokenProvider;
    private readonly IDbConnection _connection;
    
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Unsubscribe> Unsubscribed { get; set; }
    public DbSet<InvitationEvent> InvitationEvents { get; set; }
    
    protected ProviderRegistrationsDbContext() { }

    public ProviderRegistrationsDbContext(DbContextOptions<ProviderRegistrationsDbContext> options) : base(options) { }
    
    public ProviderRegistrationsDbContext(IDbConnection connection, ProviderRegistrationsSettings configuration, DbContextOptions options, AzureServiceTokenProvider azureServiceTokenProvider) : base(options)
    {
        _configuration = configuration;
        _azureServiceTokenProvider = azureServiceTokenProvider;
        _connection = connection;
    }

    public virtual Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
    {
        return Database.ExecuteSqlRawAsync(sql, parameters);
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        if (_configuration == null || _azureServiceTokenProvider == null)
        {
            return;
        }

        optionsBuilder.UseSqlServer(_connection as SqlConnection);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvitationConfiguration());
        modelBuilder.ApplyConfiguration(new InvitationEventConfiguration());
        modelBuilder.ApplyConfiguration(new UnsubscribeConfiguration());
        modelBuilder.Entity<Invitation>().HasMany(a => a.InvitationEvents).WithOne(a => a.Invitation);            
    }
}