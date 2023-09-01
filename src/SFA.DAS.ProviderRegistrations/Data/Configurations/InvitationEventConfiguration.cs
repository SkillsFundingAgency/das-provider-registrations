using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data.Configurations;

public class InvitationEventConfiguration : IEntityTypeConfiguration<InvitationEvent>
{
    public void Configure(EntityTypeBuilder<InvitationEvent> builder)
    {
        builder.Property(invitationEvent => invitationEvent.EventType).IsRequired().HasColumnType("int");
        builder.Property(invitationEvent => invitationEvent.Date).HasColumnType("datetime");            
    }
}