using System.Collections.Generic;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationsViewModel
    {
        public string SortColumn { get; set; }

        public string SortDirection { get; set; }

        public string SortedByHeaderClassName { get; set; }

        public const string HeaderClassName = "das-table__sort";

        public List<InvitationViewModel> Invitations { get; set; }

        public void SortedByHeader()
        {
            SortedByHeaderClassName += HeaderClassName;
            if (SortDirection == "Desc")
            {
                SortedByHeaderClassName += " das-table__sort--desc";
            }
            else
            {
                SortedByHeaderClassName += " das-table__sort--asc";
            }
        }
    }
}
