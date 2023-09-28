using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Services
{
    public class TrainingProviderApiClientTest
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private MockHttpMessageHandler _mockHttp = null!;
        private Fixture _fixture = null!;
        private TrainingProviderApiClient _sut = null!;
        private TrainingProviderApiClientConfiguration _settings = null!;
        private Mock<ILogger<TrainingProviderApiClient>> _logger = null!;
        private ITrainingProviderApiClientFactory _factory = null!;
        private Mock<IConfiguration> _configuration = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttp = new MockHttpMessageHandler();
            _logger = new Mock<ILogger<TrainingProviderApiClient>>();
            _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "EnvironmentName")]).Returns("LOCAL");
            _settings = _fixture
                .Build<TrainingProviderApiClientConfiguration>()
                .With(x => x.ApiBaseUrl, OuterApiBaseAddress)
                .With(x => x.IdentifierUri, "")
                .Create();
            
            _factory = new TrainingProviderApiClientFactory(new HttpClient(_mockHttp)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            }, _settings, _configuration.Object);
            _sut = new TrainingProviderApiClient(_factory, _settings, _logger.Object);
        }

        [Test]
        public async Task When_ProviderDetails_Found_Then_Data_FromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetProviderSummaryResult>();

            _mockHttp.When($"{OuterApiBaseAddress}/api/providers/{ukprn}")
                .Respond("application/json",
                    JsonSerializer.Serialize(expected));

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task When_ProviderDetails_NotFound_Then_Data_FromOuterApiIsNotFound()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            _mockHttp.When($"{OuterApiBaseAddress}/api/providers/{ukprn}")
                .Respond("application/json",
                    JsonSerializer.Serialize((GetProviderSummaryResult)null));

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeNull();
        }

        [Test]
        public async Task When_ProviderDetails_InternalServerError_Then_Data_FromOuterApiIsNotFound()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();

            var error = @"{""Code"":""ServiceUnavailable"", ""Message"" : ""Service Unavailable.""}";

            _mockHttp.When(HttpMethod.Get, $"{OuterApiBaseAddress}/api/providers/{ukprn}")
                .Respond(HttpStatusCode.InternalServerError, "application/json", JsonSerializer.Serialize(error));

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeNull();
        }
    }
}
