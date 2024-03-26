using System.ComponentModel.DataAnnotations;
using SFA.DAS.EmailValidationService;

namespace SFA.DAS.ProviderRegistrations.Web.Validation;

public class MailAddressAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult(ErrorMessage);
        }
        
        var isValid = value.ToString().IsAValidEmailAddress();

        return isValid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
}