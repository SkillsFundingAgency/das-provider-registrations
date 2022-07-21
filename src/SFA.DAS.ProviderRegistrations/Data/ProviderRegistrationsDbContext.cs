using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data
{
    public class ProviderRegistrationsDbContext : DbContext
    {
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Unsubscribe> Unsubscribed { get; set; }
        public DbSet<InvitationEvent> InvitationEvents { get; set; }

        public ProviderRegistrationsDbContext(DbContextOptions<ProviderRegistrationsDbContext> options) : base(options)
        {
        }

        protected ProviderRegistrationsDbContext()
        {
        }

        public virtual Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvitationConfiguration());
            modelBuilder.ApplyConfiguration(new InvitationEventConfiguration());
            modelBuilder.ApplyConfiguration(new UnsubscribeConfiguration());
            modelBuilder.Entity<Invitation>().HasMany(a => a.InvitationEvents).WithOne(a => a.Invitation);            
        }
    }
}