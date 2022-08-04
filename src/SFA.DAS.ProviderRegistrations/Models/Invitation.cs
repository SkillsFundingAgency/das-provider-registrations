using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public class Invitation : Entity
    {
        public long Id { get; set; }

        public Guid Reference { get; private set; }

        public long Ukprn { get; private set; }

        public string UserRef { get; private set; }

        public string EmployerOrganisation { get; private set; }

        public string EmployerFirstName { get; private set; }

        public string EmployerLastName { get; private set; }

        public string EmployerEmail { get; private set; }

        public int Status { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime UpdatedDate { get; private set; }

        public string ProviderOrganisationName { get; private set; }

        public string ProviderUserFullName { get; private set; }

        public virtual ICollection<InvitationEvent> InvitationEvents { get; set; } = new List<InvitationEvent>();

        public Invitation(Guid reference, long ukprn, string userRef, string employerOrganisation, string employerFirstName, string employerLastName, string employerEmail, int status, 
            DateTime createdDate, DateTime updatedDate, string providerOrgName, string providerUserFullName, List<InvitationEvent> invitationEvents)
        {
            Reference = reference;
            Ukprn = ukprn;
            UserRef = userRef;
            EmployerOrganisation = employerOrganisation;
            EmployerFirstName = employerFirstName;
            EmployerLastName = employerLastName;
            EmployerEmail = employerEmail;
            Status = status;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            ProviderOrganisationName = providerOrgName;
            ProviderUserFullName = providerUserFullName;
            InvitationEvents = invitationEvents;
        }

        private Invitation()
        {
        }

        public void UpdateStatus(int status, DateTime updated)
        {
            Status = status;
            UpdatedDate = updated;
        }

        public void UpdateInvitation(string employerOrganisation, string employerFirstName, string employerLastName, int status, DateTime updated)
        {
            EmployerOrganisation = employerOrganisation;
            EmployerFirstName = employerFirstName;
            EmployerLastName = employerLastName; 
            Status = status;
            UpdatedDate = updated;
        }
    }
}
