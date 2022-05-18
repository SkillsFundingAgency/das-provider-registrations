using System;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationViewModel
    {
        public Guid Reference { get; set; }

        public string EmployerOrganisation { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public string Name => $"{EmployerFirstName} {EmployerLastName}";

        public string EmployerEmail { get; set; }

        public string State { get; set; }

        public DateTime SentDate { get; set; }

        public bool ShowLink => State == "Account creation not started";
    }
}
