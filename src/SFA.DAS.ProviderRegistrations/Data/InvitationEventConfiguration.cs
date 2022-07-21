using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Data
{
    public class InvitationEventConfiguration : IEntityTypeConfiguration<InvitationEvent>
    {
        public void Configure(EntityTypeBuilder<InvitationEvent> builder)
        {
            builder.Property(a => a.EventType).IsRequired().HasColumnType("int");
            builder.Property(a => a.Date).HasColumnType("datetime");            
        }
    }
}
