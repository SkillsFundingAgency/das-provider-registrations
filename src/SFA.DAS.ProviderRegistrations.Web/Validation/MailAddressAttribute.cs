using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SFA.DAS.ProviderRegistrations.Web.Validation
{
    public class MailAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var emailAddress = value.ToString().Trim();
                if (emailAddress.Contains(' ')) return new ValidationResult(ErrorMessage); 

                var mailAddress = new MailAddress(emailAddress);
                return mailAddress.Address == emailAddress ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            }
            catch 
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}