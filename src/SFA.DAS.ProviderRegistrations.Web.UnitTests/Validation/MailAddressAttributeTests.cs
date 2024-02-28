using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Web.Validation;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Validation;

public class EmailValidationTests
{
    [TestCaseSource(typeof(ValidEmailCases))]
    public void EmailShouldBeValid(string email)
    {
        var validator = new MailAddressAttribute();
        validator.IsValid(email).Should().BeTrue();
    }

    [TestCaseSource(typeof(InvalidEmailCases))]
    public void EmailShouldBeInvalid(string email)
    {
        var validator = new MailAddressAttribute();
        validator.IsValid(email).Should().BeFalse();
    }
}