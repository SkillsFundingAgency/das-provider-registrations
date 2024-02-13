namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand;

public class AddResendInvitationCommand : IRequest
{
    public long InvitationId { get; }
    public DateTime InvitationReSentDate { get; }
        

    public AddResendInvitationCommand(long invitationId, DateTime invitationReSentDate)
    {
        InvitationId = invitationId;
        InvitationReSentDate = invitationReSentDate;
    }
}