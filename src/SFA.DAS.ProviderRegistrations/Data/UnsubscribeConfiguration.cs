using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data
{
    public class UnsubscribeConfiguration : IEntityTypeConfiguration<Unsubscribe>
    {
        public void Configure(EntityTypeBuilder<Unsubscribe> builder)
        {
            builder.HasKey(a => new { a.EmailAddress, a.Ukprn });
            builder.Property(a => a.EmailAddress).IsRequired().HasColumnType("varchar(255)");
            builder.Property(a => a.Ukprn).IsRequired().HasColumnType("bigint");
        }
    }
}