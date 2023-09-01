namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationViewModel
    {
        public long Id { get; set; }

        public Guid Reference { get; set; }

        public string EmployerOrganisation { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public string Name => $"{EmployerFirstName} {EmployerLastName}";

        public string EmployerEmail { get; set; }        

        public InvitationStatusViewModel Status { get; set; }

        public DateTime SentDate { get; set; }

        public bool ShowResendInvitationLink => Status == InvitationStatusViewModel.InvitationSent;

        public bool ShowViewStatusLink => Status == InvitationStatusViewModel.AccountStarted ||
                                          Status == InvitationStatusViewModel.PayeSchemeAdded ||
                                          Status == InvitationStatusViewModel.LegalAgreementSigned ||
                                          Status == InvitationStatusViewModel.InvitationComplete;        
    }
}
