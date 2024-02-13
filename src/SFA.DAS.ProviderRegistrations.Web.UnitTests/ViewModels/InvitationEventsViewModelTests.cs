using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Web.ViewModels;
using System;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests.ViewModels
{
    [TestFixture]
    internal class InvitationEventsViewModelTests
    {
        [Test]
        public void PayeSchemeAddedDate_Is_Shown_Correctly()
        {
            var dateTimeNow = DateTime.UtcNow;
            var viewModel = new InvitationEventsViewModel();
            viewModel.InvitationEvents = new System.Collections.Generic.List<InvitationEventViewModel>
            {
                new InvitationEventViewModel{ EventType = EventTypeViewModel.PayeSchemeAdded, Date = dateTimeNow}
            };

            Assert.That(dateTimeNow.ToString(InvitationEventsViewModel.DateFormat), Is.EqualTo(viewModel.PayeSchemeAddedStatus));
        }

        [TestCase(InvitationStatusViewModel.InvitationSent, "PAYE scheme not added")]
        [TestCase(InvitationStatusViewModel.AccountStarted, "PAYE scheme not added")]
        [TestCase(InvitationStatusViewModel.PayeSchemeAdded, "Added")]
        [TestCase(InvitationStatusViewModel.LegalAgreementSigned, "Added")]
        [TestCase(InvitationStatusViewModel.InvitationComplete, "Added")]
        public void PayeSchemeAddedStatus_Is_Shown_Correctly_When_PayeSchemeAdded_Event_Is_Not_Received(InvitationStatusViewModel status, string textToDisplay)
        {
            var dateTimeNow = DateTime.UtcNow;
            var viewModel = new InvitationEventsViewModel();
            viewModel.Invitation = new InvitationViewModel
            {
                Status = status
            };

            Assert.That(textToDisplay, Is.EqualTo(viewModel.PayeSchemeAddedStatus));
        }

        [Test]
        public void AccountCreationStartedDate_Is_Shown_Correctly()
        {
            var dateTimeNow = DateTime.UtcNow;
            var viewModel = new InvitationEventsViewModel();
            viewModel.InvitationEvents = new System.Collections.Generic.List<InvitationEventViewModel>
            {
                new InvitationEventViewModel{ EventType = EventTypeViewModel.AccountStarted, Date = dateTimeNow}
            };

            Assert.That(dateTimeNow.ToString(InvitationEventsViewModel.DateFormat), Is.EqualTo(viewModel.AccountCreationStartedStatus));
        }

        [TestCase(InvitationStatusViewModel.InvitationSent, "Account creation not started")]
        [TestCase(InvitationStatusViewModel.AccountStarted, "Started")]
        [TestCase(InvitationStatusViewModel.PayeSchemeAdded, "Started")]
        [TestCase(InvitationStatusViewModel.LegalAgreementSigned, "Started")]
        [TestCase(InvitationStatusViewModel.InvitationComplete, "Started")]
        public void GetAccountCreationStartedText_Is_Shown_Correctly_When_AccountStarted_Event_Is_Not_Received(InvitationStatusViewModel status, string textToDisplay)
        {
            var viewModel = new InvitationEventsViewModel();
            viewModel.Invitation = new InvitationViewModel
            {
                Status = status
            };

            Assert.That(textToDisplay, Is.EqualTo(viewModel.AccountCreationStartedStatus));
        }

        [Test]
        public void AgreementAcceptedDate_Is_Shown_Correctly()
        {
            var dateTimeNow = DateTime.UtcNow;
            var viewModel = new InvitationEventsViewModel();
            viewModel.InvitationEvents = new System.Collections.Generic.List<InvitationEventViewModel>
            {
                new InvitationEventViewModel{ EventType = EventTypeViewModel.LegalAgreementSigned, Date = dateTimeNow}
            };

            Assert.That(dateTimeNow.ToString(InvitationEventsViewModel.DateFormat), Is.EqualTo(viewModel.AgreementAcceptedStatus));
        }

        [TestCase(InvitationStatusViewModel.InvitationSent, "Legal agreement not accepted")]
        [TestCase(InvitationStatusViewModel.AccountStarted, "Legal agreement not accepted")]
        [TestCase(InvitationStatusViewModel.PayeSchemeAdded, "Legal agreement not accepted")]
        [TestCase(InvitationStatusViewModel.LegalAgreementSigned, "Accepted")]
        [TestCase(InvitationStatusViewModel.InvitationComplete, "Accepted")]
        public void AgreementAcceptedText_Is_Shown_Correctly_When_LegalAgreementSigned_Event_Is_Not_Received(InvitationStatusViewModel status, string textToDisplay)
        {
            var viewModel = new InvitationEventsViewModel();
            viewModel.Invitation = new InvitationViewModel
            {
                Status = status
            };

            Assert.That(textToDisplay, Is.EqualTo(viewModel.AgreementAcceptedStatus));
        }
    }
}
