using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.Property(invitation => invitation.Reference).ValueGeneratedNever();
        builder.Property(invitation => invitation.Ukprn).IsRequired().HasColumnType("bigint");
        builder.Property(invitation => invitation.UserRef).HasColumnType("varchar(255)");
        builder.Property(invitation => invitation.EmployerOrganisation).IsRequired().HasColumnType("varchar(255)");
        builder.Property(invitation => invitation.EmployerFirstName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(invitation => invitation.EmployerLastName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(invitation => invitation.EmployerEmail).IsRequired().HasColumnType("varchar(255)");
        builder.Property(invitation => invitation.Status).IsRequired().HasColumnType("int");
        builder.Property(invitation => invitation.CreatedDate).HasColumnType("datetime");
        builder.Property(invitation => invitation.UpdatedDate).HasColumnType("datetime");
        builder.Property(invitation => invitation.ProviderOrganisationName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(invitation => invitation.ProviderUserFullName).IsRequired().HasColumnType("varchar(255)");
        
        builder.HasMany(invitation => invitation.InvitationEvents).WithOne(a => a.Invitation);            
    }
}