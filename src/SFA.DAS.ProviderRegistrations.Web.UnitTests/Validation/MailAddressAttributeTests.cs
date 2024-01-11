using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Web.Validation;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.Validation
{
    [TestFixture]
    class MailAddressAttributeTests
    {
        [TestCase("email@example.com", true)]
        [TestCase("EmAiL@Example.COM", true)]
        [TestCase("firstname.lastname @example.com", false)]
        [TestCase("firstname.lastname@example.com", true)]
        [TestCase("email @subdomain.example.com", false)]
        [TestCase("firstname+lastname @example.com", false)]
        [TestCase("firstname+lastname@example.com", true)]
        [TestCase("email@123.123.123.123", true)]
        [TestCase("email@[123.123.123.123]", true)]
        [TestCase("\"email\"@example.com", true)]
        [TestCase("1234567890@example.com", true)]
        [TestCase("email@example-one.com", true)]
        [TestCase("_______@example.com", true)]
        [TestCase("email@example.name", true)]
        [TestCase("em,ail@example.name", false)]
        [TestCase("e;mail@example.name", false)]
        [TestCase("email@example.museum", true)]
        [TestCase("email@example.co.jp", true)]
        [TestCase("firstname-lastname @example.com", false)]
        [TestCase("plainaddress", false)]
        [TestCase("#@%^%#$@#$@#.com", false)]
        [TestCase("@example.com", false)]
        [TestCase("Joe Smith <email @example.com>", false)]
        [TestCase("email.example.com", false)]
        [TestCase("email@example @example.com", false)]
        [TestCase(".email @example.com", false)]
        [TestCase("email.@example.com", true)]
        [TestCase("email..email @example.com", false)]
        [TestCase("email@example.com (Joe Smith)", false)]
        [TestCase("email@-example.com", true)]
        [TestCase("email@example.web", true)]
        [TestCase("email @example..com", false)]
        public void ValidateEmailAddress(string emailAddress, bool expectedResult)
        {
            var validator = new MailAddressAttribute();
            Assert.That(validator.IsValid(emailAddress), Is.EqualTo(expectedResult));
        }
    }
}
