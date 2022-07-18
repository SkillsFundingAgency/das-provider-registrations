using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.AutoFixture
{
    public class DomainAutoDataAttribute : AutoDataAttribute
    {
        public DomainAutoDataAttribute() : base(() =>
        {
            var fixture = new Fixture();

            fixture
                .Customize(new DomainCustomizations())
                .Customize<BindingInfo>(c => c.OmitAutoProperties());

            return fixture;
        })
        {
        }
    }

    public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DomainInlineAutoDataAttribute(params object[] arguments) : base(() =>
        {
            var fixture = new Fixture();

            fixture
                .Customize(new DomainCustomizations())
                .Customize<BindingInfo>(c => c.OmitAutoProperties());

            return fixture;
        }, arguments)
        {
        }
    }
}
