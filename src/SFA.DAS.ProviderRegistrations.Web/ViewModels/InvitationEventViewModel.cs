namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationEventViewModel
    {
        public long InvitationId { get; set; }       

        public DateTime? Date { get; set; }

        public EventTypeViewModel EventType { get; set; }
    }
}
