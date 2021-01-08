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
                new MailAddress(value as string);
                return ValidationResult.Success;
            }
            catch (FormatException ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}