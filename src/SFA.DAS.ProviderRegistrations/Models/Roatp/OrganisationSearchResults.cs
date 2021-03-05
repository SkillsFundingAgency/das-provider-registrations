using System.Collections.Generic;

namespace SFA.DAS.ProviderRegistrations.Models
{
    public class OrganisationSearchResults
    {
        public List<Organisation> SearchResults { get; set; }

            public int TotalCount { get; set; }
    }
    
}