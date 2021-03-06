using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data
{
    public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.Property(a => a.Reference).ValueGeneratedNever();
            builder.Property(a => a.Ukprn).IsRequired().HasColumnType("bigint");
            builder.Property(a => a.UserRef).HasColumnType("varchar(255)");
            builder.Property(a => a.EmployerOrganisation).IsRequired().HasColumnType("varchar(255)");
            builder.Property(a => a.EmployerFirstName).IsRequired().HasColumnType("varchar(255)");
            builder.Property(a => a.EmployerLastName).IsRequired().HasColumnType("varchar(255)");
            builder.Property(a => a.EmployerEmail).IsRequired().HasColumnType("varchar(255)");
            builder.Property(a => a.Status).IsRequired().HasColumnType("int");
            builder.Property(a => a.CreatedDate).HasColumnType("datetime");
            builder.Property(a => a.UpdatedDate).HasColumnType("datetime");
            builder.Property(a => a.ProviderOrganisationName).IsRequired().HasColumnType("varchar(255)");
            builder.Property(a => a.ProviderUserFullName).IsRequired().HasColumnType("varchar(255)");
        }
    }
}