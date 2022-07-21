using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRegistrations.Web.Validation;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class NewEmployerUserViewModel
    {
        [Display(Name = "employer organisation", Order = 0)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z0-9 '-]+$", ErrorMessage = "Employer organisation must only include letters a to z, numbers 0 to 9, hyphens, spaces and apostrophes")]
        [Required(ErrorMessage = "Enter an employer organisation")]
        public string EmployerOrganisation { get; set; }

        [Display(Name = "employer first name", Order = 1)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z '-]+$", ErrorMessage = "Employer first name must only include letters a to z, hyphens, spaces and apostrophes")]
        [Required(ErrorMessage = "Enter an employer first name")]
        public string EmployerFirstName { get; set; }

        [Display(Name = "employer last name", Order = 2)]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z '-]+$", ErrorMessage = "Employer last name must only include letters a to z, hyphens, spaces and apostrophes")]
        [Required(ErrorMessage = "Enter an employer last name")]
        public string EmployerLastName { get; set; }

        [Display(Name = "employer email address", Order = 3)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Enter an employer email address")]
        [MailAddress(ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        [Unsubscribed(ErrorMessage = "Enter an email address that has not been unsubscribed from invitation emails")]
        [InUse(ErrorMessage = "Enter an email address that is not already being used on an existing account")]
        public string EmployerEmailAddress { get; set; }

        public string ProviderId { get; set; }

        public bool ResendInvitation { get; set; }

        public bool Unsubscribed { get; set; }

        public Guid Reference { get; set; }
        
        public long? InvitationId { get; set; }
    }
}