using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data.Configurations;

public class UnsubscribeConfiguration : IEntityTypeConfiguration<Unsubscribe>
{
    public void Configure(EntityTypeBuilder<Unsubscribe> builder)
    {
        builder.HasKey(unsubscribe => new { unsubscribe.EmailAddress, unsubscribe.Ukprn });
        builder.Property(unsubscribe => unsubscribe.EmailAddress).IsRequired().HasColumnType("varchar(255)");
        builder.Property(unsubscribe => unsubscribe.Ukprn).IsRequired().HasColumnType("bigint");
    }
}