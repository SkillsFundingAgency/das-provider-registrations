using System;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationEventViewModel
    {
        public long InvitationId { get; set; }       

        public DateTime? Date { get; set; }

        public string EventState { get; set; }
    }
}
